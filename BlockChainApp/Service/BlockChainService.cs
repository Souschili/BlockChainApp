using BlockChainApp.Model;
using BlockChainApp.Service.Interfaces;

namespace BlockChainApp.Service
{
    /// <summary>
    /// Provides functionality to manage a simple in-memory blockchain.
    /// </summary>
    public class BlockChainService
    {
        private readonly ITimeService _timeService;
        private readonly List<BlockModel> _blocks = new();

        /// <summary>
        /// Gets a read-only collection of all blocks in the blockchain.
        /// </summary>
        public IReadOnlyCollection<BlockModel> Blocks => _blocks.AsReadOnly();

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockChainService"/> class
        /// with a genesis block and internal time service.
        /// </summary>
        public BlockChainService()
        {
            _timeService = new TimeService();
            _blocks.Add(new BlockModel(0, "Genesis Block"));
        }

        /// <summary>
        /// Validates the entire blockchain by verifying the integrity and proper linkage
        /// of each block. Checks hashes and tampering status of all consecutive pairs of blocks.
        /// </summary>
        /// <returns>
        /// <c>true</c> if all blocks are valid and properly linked; otherwise, <c>false</c>.
        /// </returns>
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
        /// Adds a new block to the blockchain with the specified data.
        /// </summary>
        /// <param name="data">The data to store in the block.</param>
        /// <returns>
        /// <c>true</c> if the block was successfully created and added; otherwise, <c>false</c>.
        /// </returns>
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
        /// Determines whether two blocks are correctly linked and consistent in structure.
        /// </summary>
        /// <param name="previous">The preceding block in the chain.</param>
        /// <param name="current">The block to validate as the next in sequence.</param>
        /// <returns>
        /// <c>true</c> if the blocks form a valid link and both timestamps are not in the future;
        /// otherwise, <c>false</c>.
        /// </returns>
        public bool AreLinkedBlocksValid(BlockModel previous, BlockModel current)
        {
            if (previous == null || current == null || current.Previous == null)
                return false;

            return previous.Index + 1 == current.Index &&
                   previous.Hash == current.Previous.Hash &&
                   previous.CreatedOn <= _timeService.GetUtcTime() &&
                   current.CreatedOn <= _timeService.GetUtcTime();
        }
    }
}
