namespace OrderRecyclerView.Helpers
{
    public interface IItemTouchHelperAdapter
    {
        bool OnItemMove(int fromPosition, int toPosition);
        void OnItemDismiss(int position);
    }
}