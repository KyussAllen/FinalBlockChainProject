using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        Wallet kyussWallet = new Wallet();
        Wallet jesseWallet = new Wallet();

        Blockchain blockchain = new Blockchain(4);

        Transaction transaction1 = new Transaction(kyussWallet.PublicKey, jesseWallet.PublicKey, 10);
        transaction1.SignTransaction(kyussWallet);

        Transaction transaction2 = new Transaction(jesseWallet.PublicKey, kyussWallet.PublicKey, 5);
        transaction2.SignTransaction(jesseWallet);

        List<Transaction> transactions = new List<Transaction> { transaction1, transaction2 };

        blockchain.AddBlock(transactions);

        foreach (Block block in blockchain.Chain)
        {
            Console.WriteLine($"Index: {block.Index}");
            Console.WriteLine($"Previous Hash: {block.PreviousHash}");
            Console.WriteLine($"Merkle Root: {block.MerkleRoot}");
            Console.WriteLine($"Hash: {block.Hash}");
            Console.WriteLine("Transactions:");
            foreach (var transaction in block.Transactions)
            {
                Console.WriteLine($"  {transaction}");
            }
            Console.WriteLine();
        }

        Console.WriteLine($"Is blockchain valid? {blockchain.IsChainValid()}");
    }
}