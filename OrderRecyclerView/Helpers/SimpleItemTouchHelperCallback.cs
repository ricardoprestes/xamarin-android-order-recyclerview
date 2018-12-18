using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using Android.Views;
using Android.Widget;

namespace OrderRecyclerView.Helpers
{
    public class SimpleItemTouchHelperCallback : ItemTouchHelper.Callback
    {
        public static float ALPHA_FULL = 1.0f;
        private IItemTouchHelperAdapter Adapter;

        public SimpleItemTouchHelperCallback(IItemTouchHelperAdapter adapter)
        {
            Adapter = adapter;
        }

        public override bool IsLongPressDragEnabled => true;
        public override bool IsItemViewSwipeEnabled => true;

        public override int GetMovementFlags(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder)
        {
            if (recyclerView.GetLayoutManager() is GridLayoutManager)
            {
                int dragFlags = ItemTouchHelper.Up | ItemTouchHelper.Down | ItemTouchHelper.Left | ItemTouchHelper.Right;
                int swipeFlags = 0;
                return MakeMovementFlags(dragFlags, swipeFlags);
            }
            else
            {
                int dragFlags = ItemTouchHelper.Up | ItemTouchHelper.Down;
                int swipeFlags = ItemTouchHelper.Start | ItemTouchHelper.End;
                return MakeMovementFlags(dragFlags, swipeFlags);
            }
        }

        public override bool OnMove(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, RecyclerView.ViewHolder target)
        {
            if (viewHolder.ItemViewType != target.ItemViewType)
                return false;

            Adapter.OnItemMove(viewHolder.AdapterPosition, target.AdapterPosition);
            return true;
        }

        public override void OnSwiped(RecyclerView.ViewHolder viewHolder, int direction)
        {
            Adapter.OnItemDismiss(viewHolder.AdapterPosition);
        }

        public override void OnChildDraw(Canvas c, RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, float dX, float dY, int actionState, bool isCurrentlyActive)
        {
            if (actionState == ItemTouchHelper.ActionStateSwipe)
            {
                float alpha = ALPHA_FULL - Math.Abs(dX) / (float)viewHolder.ItemView.Width;
                viewHolder.ItemView.Alpha = alpha;
                viewHolder.ItemView.TranslationX = dX;
            }
            else
                base.OnChildDraw(c, recyclerView, viewHolder, dX, dY, actionState, isCurrentlyActive);
        }

        public override void OnSelectedChanged(RecyclerView.ViewHolder viewHolder, int actionState)
        {
            if (actionState != ItemTouchHelper.ActionStateIdle)
            {
                if (viewHolder is IItemTouchHelperViewHolder) {
                    IItemTouchHelperViewHolder itemViewHolder = (IItemTouchHelperViewHolder)viewHolder;
                    itemViewHolder.OnItemSelected();
                }
            }
            base.OnSelectedChanged(viewHolder, actionState);
        }

        public override void ClearView(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder)
        {
            base.ClearView(recyclerView, viewHolder);
            viewHolder.ItemView.Alpha = ALPHA_FULL;

            if (viewHolder is IItemTouchHelperViewHolder) {
                IItemTouchHelperViewHolder itemViewHolder = (IItemTouchHelperViewHolder)viewHolder;
                itemViewHolder.OnItemClear();
            }
        }
    }
}