using BlockChainApp.Model;

namespace BlockChainApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var genesis = new BlockModel(0, "Genesis block", null);
            var block1 = new BlockModel(1, "Alice pays Bob 5", genesis);
            var block2 = new BlockModel(2, "Bob pays Charlie 2", block1);
            var block3 = new BlockModel(3, "Charlie pays Dave 1", block2);

            var chain = new[] { genesis, block1, block2, block3 };

            foreach (var block in chain)
            {
                Console.WriteLine($"Index: {block.Index}");
                Console.WriteLine($"Data: {block.Data}");
                Console.WriteLine($"CreatedOn: {block.CreatedOn:O}");
                Console.WriteLine($"Previous Hash: {block.Previous?.Hash ?? "None"}");
                Console.WriteLine($"Hash: {block.Hash}");
                Console.WriteLine(new string('-', 50));
            }

        }
    }
}
