public class Blockchain
{
    public List<Block> Chain { get; set; }
    public int Difficulty { get; set; }

    public Blockchain(int difficulty)
    {
        Chain = new List<Block>();
        Difficulty = difficulty;
        AddGenesisBlock();
    }

    private void AddGenesisBlock()
    {
        List<Transaction> genesisTransactions = new List<Transaction>
        {
            new Transaction("Genesis", "Kyuss", 1000) // Initial funds for Kyuss
        };
        Chain.Add(new Block(0, "0", genesisTransactions, DateTime.Now));
    }

    public Block GetLatestBlock()
    {
        return Chain[Chain.Count - 1];
    }

    public void AddBlock(List<Transaction> transactions)
    {
        foreach (var transaction in transactions)
        {
            if (!transaction.IsValid())
            {
                throw new Exception("Invalid transaction detected!");
            }
        }

        Block latestBlock = GetLatestBlock();
        Block newBlock = new Block(latestBlock.Index + 1, latestBlock.Hash, transactions, DateTime.Now);
        newBlock.MineBlock(Difficulty);
        Chain.Add(newBlock);
    }

    public bool IsChainValid()
    {
        for (int i = 1; i < Chain.Count; i++)
        {
            Block currentBlock = Chain[i];
            Block previousBlock = Chain[i - 1];

            if (currentBlock.Hash != currentBlock.CalculateHash())
            {
                Console.WriteLine("Current block hash is invalid!");
                return false;
            }

            if (currentBlock.PreviousHash != previousBlock.Hash)
            {
                Console.WriteLine("Previous block hash is invalid!");
                return false;
            }

            foreach (var transaction in currentBlock.Transactions)
            {
                if (!transaction.IsValid())
                {
                    Console.WriteLine("Invalid transaction detected!");
                    return false;
                }
            }
        }
        return true;
    }
}