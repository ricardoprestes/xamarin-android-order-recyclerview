using Android.Support.V7.Widget;

namespace OrderRecyclerView.Helpers
{
    public interface IOnStartDragListener
    {
        void OnStartDrag(RecyclerView.ViewHolder viewHolder);
    }
}