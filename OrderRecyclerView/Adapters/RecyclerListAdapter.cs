using Android.Content;
using Android.Graphics;
using Android.Support.V4.View;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using OrderRecyclerView.Helpers;
using System;
using System.Collections.Generic;

namespace OrderRecyclerView.Adapters
{
    public class RecyclerListAdapter : RecyclerView.Adapter, IItemTouchHelperAdapter
    {
        public List<string> Items { get; set; }

        private IOnStartDragListener DragStartListener;

        public RecyclerListAdapter(Context context, IOnStartDragListener dragStartListener, List<string> items)
        {
            DragStartListener = dragStartListener;
            Items = items;
        }

        public override int ItemCount => Items.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var holder = viewHolder as ItemViewHolder;
            holder.textView.Text = Items[position];
            holder.handleView.SetOnTouchListener(new ItemTouchListener(DragStartListener, holder));
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.item_list, parent, false);
            ItemViewHolder itemViewHolder = new ItemViewHolder(view);
            return itemViewHolder;
        }

        public void OnItemDismiss(int position)
        {
            Items.RemoveAt(position);
            NotifyItemRemoved(position);
        }

        public bool OnItemMove(int fromPosition, int toPosition)
        {
            Swap(fromPosition, toPosition);
            NotifyItemMoved(fromPosition, toPosition);
            return true;
        }

        public void Swap(int indexA, int indexB)
        {
            var tmp = Items[indexA];
            Items[indexA] = Items[indexB];
            Items[indexB] = tmp;
        }
    }

    public class ItemViewHolder : RecyclerView.ViewHolder, IItemTouchHelperViewHolder
    {
        public TextView textView;
        public ImageView handleView;

        public ItemViewHolder(View itemView) : base(itemView)
        {
            textView = itemView.FindViewById< TextView>(Resource.Id.text);
            handleView = itemView.FindViewById< ImageView>(Resource.Id.handle);
        }

        public void OnItemClear()
        {
            ItemView.SetBackgroundColor(Color.White);
        }

        public void OnItemSelected()
        {
            ItemView.SetBackgroundColor(Color.LightGray);
        }
    }

    public class ItemTouchListener : Java.Lang.Object, View.IOnTouchListener
    {
        private IOnStartDragListener DragStartListener;
        private ItemViewHolder ViewHolder;

        public ItemTouchListener(IOnStartDragListener dragStartListener, ItemViewHolder viewHolder)
        {
            DragStartListener = dragStartListener;
            ViewHolder = viewHolder;
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            if (e.Action == MotionEventActions.Down)
            {
                DragStartListener.OnStartDrag(ViewHolder);
            }
            return false;
        }
    }
}