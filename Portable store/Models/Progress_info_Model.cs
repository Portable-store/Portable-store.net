namespace Portable_store.Models
{
    public class Progress_info_Model
    {
        public Progress_info_Model(uint max_count, uint current_count = 0, string details = "")
        {
            Current_count = current_count;
            Max_count = max_count;
            Details = details ?? "";
        }

        public Progress_info_Model(int max_count, int current_count = 0, string details = "") :
            this(Convert.ToUInt32(current_count), Convert.ToUInt32(max_count), details) { }

        #region Variables
        /// <summary>
        /// The number of items processed
        /// </summary>
        public uint Current_count;
        /// <summary>
        /// The maximum number of items to process
        /// </summary>
        public uint Max_count;
        /// <summary>
        /// Current details of what is being processed
        /// </summary>
        public string Details = string.Empty;
        #endregion

        #region Properties
        /// <summary>
        /// The percentage of the current progress
        /// </summary>
        public float Percentage => Max_count == 0 ? 0 : (Current_count * 100 / Max_count);
        #endregion

        #region Methods
        public Progress_info_Model Increment(string new_details = "")
        {
            Current_count++;
            Details = new_details;

            return this;
        }

        public Progress_info_Model Update(string details, uint current_count)
        {
            Current_count = current_count;
            Details = details;

            return this;
        }

        public Progress_info_Model Update(string details, float current_count) =>
            Update(details, Convert.ToUInt32(current_count));

        public Progress_info_Model Update(string details, int current_count) =>
            Update(details, Convert.ToUInt32(current_count));
        #endregion
    }
}
