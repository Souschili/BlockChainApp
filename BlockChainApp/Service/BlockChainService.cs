using BlockChainApp.Model;

namespace BlockChainApp.Service
{
    /// <summary>
    /// Provides functionality to manage a simple blockchain.
    /// </summary>
    public class BlockChainService
    {
        private readonly List<BlockModel> _blocks = new();

        /// <summary>
        /// Gets the collection of blocks in the blockchain.
        /// </summary>
        public IReadOnlyCollection<BlockModel> Blocks => _blocks.AsReadOnly();

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockChainService"/> class with a genesis block.
        /// </summary>
        public BlockChainService()
        {
            _blocks.Add(new BlockModel(0, "Genesis Block"));
        }

        /// <summary>
        /// Validates the entire blockchain to ensure block integrity and correct link sequence.
        /// </summary>
        /// <returns><c>true</c> if the blockchain is valid; otherwise, <c>false</c>.</returns>
        public bool ValidateBlocks()
        {
            if (_blocks.Count <= 1)
                return true;

            for (int i = 1; i < _blocks.Count; i++)
            {
                var current = _blocks[i];
                var previous = _blocks[i - 1];

                if (current.IsTampered() || previous.IsTampered() || !AreLinkedBlocksValid(previous, current))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Adds a new block to the blockchain.
        /// </summary>
        /// <param name="data">The data to store in the block.</param>
        /// <returns><c>true</c> if the block was added successfully; otherwise, <c>false</c>.</returns>
        public bool AddBlock(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
                return false;

            var index = _blocks.Count;
            var lastBlock = _blocks[^1];
            var block = new BlockModel(index, data, lastBlock);
            _blocks.Add(block);

            return true;
        }

        /// <summary>
        /// Validates whether two blocks are correctly linked.
        /// </summary>
        /// <param name="previous">The previous block.</param>
        /// <param name="current">The current block.</param>
        /// <returns><c>true</c> if the blocks are linked correctly; otherwise, <c>false</c>.</returns>
        public bool AreLinkedBlocksValid(BlockModel previous, BlockModel current)
        {
            if (previous == null || current == null || current.Previous == null)
                return false;

            return previous.Index + 1 == current.Index &&
                   previous.Hash == current.Previous.Hash &&
                   previous.CreatedOn <= DateTime.UtcNow &&
                   current.CreatedOn <= DateTime.UtcNow;
        }
    }
}
