namespace TodoApiDTO.Domain
{
    #region snippet
    internal class TodoItem
    {
        public long Id { get; private set; }
        public string Name { get; private set; }
        public bool IsComplete { get; private set; }
        public string Secret { get; private set; }

        private TodoItem() { }

        public TodoItem(string name, bool isComplete)
        {
            Name = name;
            IsComplete = isComplete;
        }

        public void ChangeItem(string name, bool isComplete)
        {
            Name = name;
            IsComplete = isComplete;
        }
    }
    #endregion
}