using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BotPlayer : PlayerBase
{
    [SerializeField] private bool isRandom;
    [SerializeField, Range(0f, 1f)] private float difficultyLevel = 0.8f;

    private List<Card> playedCards = new List<Card>();
    private List<Card> centerCards = new List<Card>();
    private int totalCardsDealt = 0;

    public override void Initialize()
    {
        base.Initialize();
        _eventBus.Subscribe<GameEvents.OnPlayerPlayCard>(OnPlayerPlayCard);
        _eventBus.Subscribe<GameEvents.OnMakeMatch>(OnMakeMatch);
    }

    private void OnDisable()
    {
        _eventBus.Unsubscribe<GameEvents.OnPlayerPlayCard>(OnPlayerPlayCard);
        _eventBus.Unsubscribe<GameEvents.OnMakeMatch>(OnMakeMatch);
    }

    private void OnPlayerPlayCard(GameEvents.OnPlayerPlayCard evt)
    {
        centerCards.Add(evt.Card.CardData);
        playedCards.Add(evt.Card.CardData);
        totalCardsDealt++;
    }

    private void OnMakeMatch(GameEvents.OnMakeMatch evt)
    {
        centerCards.Clear();
    }

    internal override void Play()
    {
        base.Play();

        CardObject card;

        if (isRandom || Random.value > difficultyLevel)
            card = GetRandomCard();
        else
            card = GetBestCard();

        card.IsVisible = true;
        card.SetCardVisibility();
        PlayCard(card);
    }

    private CardObject GetRandomCard()
    {
        int i = Random.Range(0, Cards.Count);
        return Cards[i];
    }

    private CardObject GetBestCard()
    {
        // Masada kart yoksa savunma stratejisi
        if (centerCards.Count == 0)
            return GetCardForEmptyTable();

        // Pişti fırsatı var mı? (masada tek kart)
        if (centerCards.Count == 1)
            return GetCardForPistiOpportunity();

        // Normal eşleştirme stratejisi
        return GetCardForMatching();
    }

    /// <summary>
    /// Masa boşken hangi kartı oynayalım?
    /// Rakibe pişti vermemek için düşük değerli ve çok oynanan kartları tercih et
    /// </summary>
    private CardObject GetCardForEmptyTable()
    {
        CardObject bestCard = null;
        int bestScore = int.MinValue;

        foreach (var cardObj in Cards)
        {
            int score = EvaluateCardForEmptyTable(cardObj.CardData);
            if (score > bestScore)
            {
                bestScore = score;
                bestCard = cardObj;
            }
        }

        return bestCard ?? Cards[0];
    }

    private int EvaluateCardForEmptyTable(Card card)
    {
        int score = 0;
        int playedCount = CountPlayedCards(card.cardNumber);

        // Çok oynanan kartları tercih et (rakip eşleştiremez)
        score += playedCount * 15;

        // Vale (J) asla boş masaya atma - çok değerli
        if (card.cardNumber == 10)
            return -1000;

        // Değerli kartları korumaya çalış
        if (IsValuableCard(card))
            score -= 30;

        // Orta değerli kartları tercih et (4-8)
        if (card.cardNumber >= 3 && card.cardNumber <= 7)
            score += 10;

        return score;
    }

    /// <summary>
    /// Masada tek kart var - Pişti şansı!
    /// </summary>
    private CardObject GetCardForPistiOpportunity()
    {
        Card tableCard = centerCards[0];

        // Önce normal eşleşme ile pişti yap (Vale hariç)
        foreach (var cardObj in Cards)
        {
            if (cardObj.CardData.cardNumber == tableCard.cardNumber &&
                cardObj.CardData.cardNumber != 10)
            {
                return cardObj; // PİŞTİ! +10 puan
            }
        }

        // Masadaki kart değerli mi? Vale ile al
        if (IsValuableCard(tableCard))
        {
            var jack = GetJack();
            if (jack != null)
                return jack;
        }

        // Eşleşen kart yok, savunma moduna geç
        return GetDefensiveCard(tableCard);
    }

    /// <summary>
    /// Masada birden fazla kart var - normal eşleştirme
    /// </summary>
    private CardObject GetCardForMatching()
    {
        Card lastTableCard = centerCards[centerCards.Count - 1];
        int tableCardCount = centerCards.Count;

        // Toplam masa değerini hesapla
        int tableValue = CalculateTableValue();

        // 1. Öncelik: Normal eşleşme (aynı numara)
        var matchingCard = GetMatchingCard(lastTableCard.cardNumber);
        if (matchingCard != null)
        {
            // Masada çok kart veya değerli kart varsa hemen al
            if (tableCardCount >= 3 || tableValue >= 2)
                return matchingCard;

            // Az kartlı masa için de al
            return matchingCard;
        }

        // 2. Vale kullanımı - stratejik değerlendirme
        var jack = GetJack();
        if (jack != null)
        {
            // Masada 4+ kart veya 3+ puan varsa Vale kullan
            if (tableCardCount >= 4 || tableValue >= 3)
                return jack;

            // Masada değerli tekil kart varsa ve başka seçenek yoksa
            if (tableValue >= 2 && !HasBetterOption())
                return jack;
        }

        // 3. Eşleşme yok - en az zararlı kartı at
        return GetLeastValuableCard();
    }

    /// <summary>
    /// Rakibe pişti vermeyecek savunma kartı
    /// </summary>
    private CardObject GetDefensiveCard(Card tableCard)
    {
        CardObject bestCard = null;
        int bestScore = int.MinValue;

        foreach (var cardObj in Cards)
        {
            // Eşleşen kartı atlama (zaten kontrol ettik)
            if (cardObj.CardData.cardNumber == tableCard.cardNumber)
                continue;

            int score = EvaluateDefensiveCard(cardObj.CardData);
            if (score > bestScore)
            {
                bestScore = score;
                bestCard = cardObj;
            }
        }

        // Hiç uygun kart yoksa en değersiz kartı at
        return bestCard ?? GetLeastValuableCard();
    }

    private int EvaluateDefensiveCard(Card card)
    {
        int score = 0;
        int remaining = 4 - CountPlayedCards(card.cardNumber);

        // Rakibin bu kartla pişti yapma olasılığı
        // Kalan kart sayısı az = daha güvenli
        score += (4 - remaining) * 20;

        // Vale asla savunmada kullanma
        if (card.cardNumber == 10)
            return -1000;

        // Değerli kartları koruma
        if (IsValuableCard(card))
            score -= 25;

        return score;
    }

    private CardObject GetMatchingCard(int cardNumber)
    {
        // Vale hariç eşleşen kartı bul
        foreach (var cardObj in Cards)
        {
            if (cardObj.CardData.cardNumber == cardNumber && cardNumber != 10)
                return cardObj;
        }
        return null;
    }

    private CardObject GetJack()
    {
        foreach (var cardObj in Cards)
        {
            if (cardObj.CardData.cardNumber == 10)
                return cardObj;
        }
        return null;
    }

    private CardObject GetLeastValuableCard()
    {
        CardObject leastValuable = null;
        int lowestValue = int.MaxValue;

        foreach (var cardObj in Cards)
        {
            int value = GetCardValue(cardObj.CardData);

            // Vale'yi asla "değersiz" olarak atma
            if (cardObj.CardData.cardNumber == 10)
                continue;

            if (value < lowestValue)
            {
                lowestValue = value;
                leastValuable = cardObj;
            }
        }

        return leastValuable ?? Cards[0];
    }

    private bool HasBetterOption()
    {
        // Vale dışında bir eşleşme veya düşük değerli kart var mı?
        int nonJackCards = Cards.Count(c => c.CardData.cardNumber != 10);
        return nonJackCards > 1;
    }

    private int CalculateTableValue()
    {
        int value = 0;
        foreach (var card in centerCards)
        {
            value += GetCardPointValue(card);
        }
        return value;
    }

    private int GetCardPointValue(Card card)
    {
        // As = 1 puan
        if (card.cardNumber == 0)
            return 1;

        // 2♣ = 2 puan
        if (card.cardNumber == 1 && card.suit == 2)
            return 2;

        // 10♦ = 3 puan
        if (card.cardNumber == 9 && card.suit == 3)
            return 3;

        // J/Q/K = 1 puan
        if (card.cardNumber >= 10)
            return 1;

        return 0;
    }

    private int GetCardValue(Card card)
    {
        int value = GetCardPointValue(card);

        // Vale ekstra değerli (her kartı alabilir)
        if (card.cardNumber == 10)
            value += 5;

        // Kart sayma bonusu - nadir kartlar daha değerli
        int remaining = 4 - CountPlayedCards(card.cardNumber);
        value += remaining;

        return value;
    }

    private bool IsValuableCard(Card card)
    {
        // As
        if (card.cardNumber == 0)
            return true;

        // 2♣
        if (card.cardNumber == 1 && card.suit == 2)
            return true;

        // 10♦
        if (card.cardNumber == 9 && card.suit == 3)
            return true;

        // J/Q/K
        if (card.cardNumber >= 10)
            return true;

        return false;
    }

    private int CountPlayedCards(int cardNumber)
    {
        int count = 0;

        foreach (Card card in playedCards)
        {
            if (card.cardNumber == cardNumber)
                count++;
        }

        return count;
    }
}
