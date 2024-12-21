﻿namespace Ordering.Domain.ValueObjects;

public record Payment
{
    public string? CardName { get; }
    public string CardNumber { get; }
    public string Expiration { get; }
    public string CVV { get; }
    public int PaymentMethod { get; }
    
    protected Payment()
    { }

    private Payment(
        string? cardName,
        string cardNumber,
        string expiration,
        string cvv,
        int paymentMethod)
    {
        CardName = cardName;
        CardNumber = cardNumber;
        Expiration = expiration;
        CVV = cvv;
        PaymentMethod = paymentMethod;
    }

    public static Payment Of(
        string? cardName,
        string cardNumber,
        string expiration,
        string cvv,
        int paymentMethod)
    {
        ArgumentException.ThrowIfNullOrEmpty(cardName);
        ArgumentException.ThrowIfNullOrEmpty(cardNumber);
        ArgumentException.ThrowIfNullOrEmpty(cvv);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(cvv.Length, 3);
        
        return new Payment(
            cardName,
            cardNumber,
            expiration,
            cvv,
            paymentMethod
        );
    }
}