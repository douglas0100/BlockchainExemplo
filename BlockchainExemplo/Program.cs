using System.Security.Cryptography;
using System.Text;

public class Program
{
    static void Main(string[] args)
    {
        Blockchain myBlockchain = new Blockchain();

        Block block01 = new Block("Douglas Leite", "");
        Block block02 = new Block("Loana Isabelly", "");

        myBlockchain.AddBlock(block01);
        myBlockchain.AddBlock(block02);

        Console.WriteLine(myBlockchain);

        Console.WriteLine("Is blockchain valid: " + myBlockchain.IsChainValid());

        myBlockchain._chain[2].Element = "Jose Pedrosa";

        Console.WriteLine("Is blockchain valid: " + myBlockchain.IsChainValid());

    }
}

public class Block
{
    public string Element { get; set; }
    public string PreviousHash { get; set; }
    public string Hash { get; set; }

    public Block(string element, string previousHash)
    {
        Element = element;
        PreviousHash = previousHash;
        Hash = CalculateHash();
    }

    public string CalculateHash()
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(Element + PreviousHash));

            string hashCode = BuilderStringHash(bytes);

            return hashCode;
        }
    }

    private static string BuilderStringHash(byte[] bytes)
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < bytes.Length; i++)
        {
            builder.Append(bytes[i].ToString("x2"));
        }
        return builder.ToString();
    }

    public override string ToString()
    {
        return string.Format("Previous Hash: " + PreviousHash + "\r\n" + "Hash: " + Hash + "\r\n");
    }

}

public class Blockchain
{
    public List<Block> _chain;

    public Blockchain()
    {
        _chain = new List<Block>();
        _chain.Add(CreateGenesisBlock());
    }

    private Block CreateGenesisBlock()
    {
        return new Block("Douglas Leite", "0");
    }

    public Block GetLatestBlock()
    {
        return _chain[_chain.Count - 1];
    }

    public void AddBlock(Block newBlock)
    {
        newBlock.PreviousHash = GetLatestBlock().Hash;
        newBlock.Hash = newBlock.CalculateHash();
        _chain.Add(newBlock);
    }

    public bool IsChainValid()
    {
        for (int i = 1; i < _chain.Count; i++)
        {
            var currentBlock = _chain[i];
            var previousBlock = _chain[i - 1];

            if (currentBlock.Hash != currentBlock.CalculateHash())
            {
                return false;
            }
            if (currentBlock.PreviousHash != previousBlock.Hash)
            {
                return false;
            }
        }
        return true;
    }

    public override string ToString()
    {
        string result = "Chain {\r\n \r\n";
        foreach (var block in _chain)
        {
            result += block.ToString() + "\r\n";
        }
        result += "}";
        return result;
    }

}