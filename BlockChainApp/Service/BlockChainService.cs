using BlockChainApp.Model;

namespace BlockChainApp.Service
{
    /// <summary>
    /// Provides functionality to manage a simple blockchain.
    /// </summary>
    public class BlockChainService
    {
        private readonly List<BlockModel> _blocks = new List<BlockModel>();

        /// <summary>
        /// Gets the collection of blocks in the blockchain.
        /// </summary>
        public IReadOnlyCollection<BlockModel> Blocks => _blocks.AsReadOnly();

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockChainService"/> class with a genesis block.
        /// </summary>
        public BlockChainService()
        {
            _blocks.Add(new BlockModel(0, DateTime.UtcNow.ToString()));
        }

        /// <summary>
        /// Validates the entire blockchain to ensure block integrity and correct link sequence.
        /// </summary>
        /// <returns><c>true</c> if the blockchain is valid; otherwise, <c>false</c>.</returns>
        public bool ValidateBlocks()
        {
            if (_blocks.Count == 0)
                return true;

            for (int i = 1; i < _blocks.Count; i++)
            {
                var current = _blocks[i];
                var previous = _blocks[i - 1];

                if (current.IsTampered() || previous.IsTampered())
                    return false;

                if (!AreLinkedBlocksValid(previous, current))
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
            if (!string.IsNullOrEmpty(data))
            {
                var index = _blocks.Count;
                var lastBlock = _blocks.Last();
                var block = new BlockModel(index, data, lastBlock);
                _blocks.Add(block);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Validates whether two blocks are correctly linked and not tampered with.
        /// </summary>
        /// <param name="previous">The previous block.</param>
        /// <param name="current">The current block.</param>
        /// <returns><c>true</c> if the blocks are linked and valid; otherwise, <c>false</c>.</returns>
        public bool AreLinkedBlocksValid(BlockModel previous, BlockModel current)
        {
            if (previous == null || current == null)
                return false;

            if (current.Previous == null)
                return false;

            return previous.Index + 1 == current.Index &&
                   previous.Hash == current.Previous.Hash &&
                   previous.CreatedOn <= DateTime.UtcNow &&
                   current.CreatedOn <= DateTime.UtcNow &&
                   !previous.IsTampered() &&
                   !current.IsTampered();
        }
    }
}
