using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using Android.Views;
using Android.Widget;
using OrderRecyclerView.Adapters;
using OrderRecyclerView.Helpers;

namespace OrderRecyclerView
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, IOnStartDragListener
    {
        private ItemTouchHelper ItemTouchHelper;

        RecyclerView RvItems { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            RvItems = FindViewById<RecyclerView>(Resource.Id.rv_items);
            RvItems.HasFixedSize = true;

            RecyclerListAdapter adapter = new RecyclerListAdapter(this, this, new List<string> { "item 1", "item 2", "item 3", "item 4" });
            RvItems.SetAdapter(adapter);
            RvItems.SetLayoutManager(new LinearLayoutManager(this));

            ItemTouchHelper.Callback callback = new SimpleItemTouchHelperCallback(adapter);
            ItemTouchHelper = new ItemTouchHelper(callback);
            ItemTouchHelper.AttachToRecyclerView(RvItems);

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        public void OnStartDrag(RecyclerView.ViewHolder viewHolder)
        {
            ItemTouchHelper.StartDrag(viewHolder);
        }
    }
}

