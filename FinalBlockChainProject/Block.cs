using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

public class Block
{
    public int Index { get; set; }
    public string PreviousHash { get; set; }
    public List<Transaction> Transactions { get; set; }
    public DateTime Timestamp { get; set; }
    public string Hash { get; set; }
    public int Nonce { get; set; }
    public string MerkleRoot { get; set; }

    public Block(int index, string previousHash, List<Transaction> transactions, DateTime timestamp)
    {
        Index = index;
        PreviousHash = previousHash;
        Transactions = transactions;
        Timestamp = timestamp;
        Nonce = 0;
        MerkleRoot = CalculateMerkleRoot(transactions);
        Hash = CalculateHash();
    }

    public string CalculateHash()
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            string rawData = $"{Index}{PreviousHash}{MerkleRoot}{Timestamp}{Nonce}";
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }

    public string CalculateMerkleRoot(List<Transaction> transactions)
    {
        List<string> transactionHashes = new List<string>();
        foreach (var transaction in transactions)
        {
            transactionHashes.Add(CalculateTransactionHash(transaction));
        }
        return BuildMerkleTree(transactionHashes);
    }

    private string CalculateTransactionHash(Transaction transaction)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            string rawData = $"{transaction.Sender}{transaction.Receiver}{transaction.Amount}";
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }

    private string BuildMerkleTree(List<string> hashes)
    {
        if (hashes.Count == 0) return string.Empty;
        if (hashes.Count == 1) return hashes[0];

        List<string> newHashes = new List<string>();
        for (int i = 0; i < hashes.Count - 1; i += 2)
        {
            newHashes.Add(CalculateHashPair(hashes[i], hashes[i + 1]));
        }
        if (hashes.Count % 2 == 1)
        {
            newHashes.Add(CalculateHashPair(hashes[hashes.Count - 1], hashes[hashes.Count - 1]));
        }
        return BuildMerkleTree(newHashes);
    }

    private string CalculateHashPair(string hash1, string hash2)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            string rawData = $"{hash1}{hash2}";
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }

    public void MineBlock(int difficulty)
    {
        string target = new string('0', difficulty);
        while (Hash.Substring(0, difficulty) != target)
        {
            Nonce++;
            Hash = CalculateHash();
        }
        Console.WriteLine($"Block mined: {Hash}");
    }
}