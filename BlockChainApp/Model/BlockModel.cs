using System.Security.Cryptography;
using System.Text;

namespace BlockChainApp.Model
{
    /// <summary>
    /// Represents a single block in the blockchain.
    /// </summary>
    public class BlockModel
    {
        /// <summary>
        /// Gets the index of the block in the blockchain.
        /// </summary>
        public int Index { get; }

        private readonly string _hash;

        /// <summary>
        /// Gets the hash of the block.
        /// </summary>
        public string Hash => _hash;

        /// <summary>
        /// Gets the previous block in the blockchain, or <c>null</c> if this is the genesis block.
        /// </summary>
        public BlockModel? Previous { get; }

        /// <summary>
        /// Gets the data stored in the block.
        /// </summary>
        public string Data { get; }

        /// <summary>
        /// Gets the UTC timestamp when the block was created.
        /// </summary>
        public DateTime CreatedOn { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockModel"/> class.
        /// </summary>
        /// <param name="index">The index of the block.</param>
        /// <param name="data">The data to store in the block.</param>
        /// <param name="previous">The previous block in the chain; <c>null</c> for the genesis block.</param>
        public BlockModel(int index, string data, BlockModel? previous = null)
        {
            Index = index;
            Data = data;
            Previous = previous;
            CreatedOn = DateTime.UtcNow;
            _hash = CalculateHash();
        }

        /// <summary>
        /// Determines whether the block's current hash does not match the calculated hash,
        /// indicating possible tampering.
        /// </summary>
        /// <returns><c>true</c> if the block has been tampered with; otherwise, <c>false</c>.</returns>
        public bool IsTampered() => _hash != CalculateHash();

        /// <summary>
        /// Calculates the SHA256 hash of the block based on its properties.
        /// </summary>
        /// <returns>The hexadecimal string representation of the hash.</returns>
        private string CalculateHash()
        {
            // Формируем строку из всех значимых данных блока
            var raw = $"{Index}-{CreatedOn:O}-{Data}-{Previous?.Hash ?? "0"}";

            // Получаем байты из строки
            var bytes = Encoding.UTF8.GetBytes(raw);

            // Вычисляем SHA256 хеш
            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(bytes);

            // Возвращаем хеш в виде hex-строки
            return Convert.ToHexString(hash);
        }
    }
}
