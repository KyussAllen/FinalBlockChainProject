public class Transaction
{
    public string Sender { get; }
    public string Receiver { get; }
    public decimal Amount { get; }
    public string Signature { get; private set; }

    public Transaction(string sender, string receiver, decimal amount)
    {
        Sender = sender;
        Receiver = receiver;
        Amount = amount;
    }

    public void SignTransaction(Wallet wallet)
    {
        if (wallet.PublicKey != Sender)
        {
            throw new Exception("You cannot sign transactions for other wallets!");
        }
        string transactionData = $"{Sender}{Receiver}{Amount}";
        Signature = wallet.SignData(transactionData);
    }

    public bool IsValid()
    {
        if (Sender == "Genesis") return true; 

        Wallet wallet = new Wallet(); 
        string transactionData = $"{Sender}{Receiver}{Amount}";
        return wallet.VerifySignature(transactionData, Signature, Sender);
    }

    public override string ToString()
    {
        return $"{Sender} -> {Receiver}: {Amount} (Signature: {Signature})";
    }
}