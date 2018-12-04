using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Callback = Android.Support.V7.Widget.Helper.ItemTouchHelper.Callback;
using Math = System.Math;
using String = System.String;

namespace RiseAndWalk_Android.Views
{
    public abstract class SwipeControllerActions
    {

        public void onLeftClicked(int position)
        {
        }

        public void onRightClicked(int position)
        {
        }

    }

    enum ButtonsState
    {
        GONE,
        LEFT_VISIBLE,
        RIGHT_VISIBLE
    }

    class SwipeController : Callback
    {

        private bool swipeBack = false;
        private ButtonsState buttonShowedState = ButtonsState.GONE;
        private RectF buttonInstance = null;
        private RecyclerView.ViewHolder currentItemViewHolder = null;
        private SwipeControllerActions buttonsActions = null;
        private const float buttonWidth = 300;

        public SwipeController(SwipeControllerActions buttonsActions)
        {
            this.buttonsActions = buttonsActions;
        }


        public override int GetMovementFlags(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder)
        {
            return makeMovementFlags(0, LEFT | RIGHT);
        }

        public override bool OnMove(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder,
            RecyclerView.ViewHolder target)
        {
            return false;
        }

        public override void OnSwiped(RecyclerView.ViewHolder viewHolder, int direction)
        {
        }

        public override int ConvertToAbsoluteDirection(int flags, int layoutDirection)
        {
            if (swipeBack)
            {
                swipeBack = buttonShowedState != ButtonsState.GONE;
                return 0;
            }

            return base.ConvertToAbsoluteDirection(flags, layoutDirection);
        }

        public override void OnChildDraw(Canvas c, RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder,
            float dX, float dY, int actionState, bool isCurrentlyActive)
        {
            if (actionState == ACTION_STATE_SWIPE)
            {
                if (buttonShowedState != ButtonsState.GONE)
                {
                    if (buttonShowedState == ButtonsState.LEFT_VISIBLE) dX = Math.Max(dX, buttonWidth);
                    if (buttonShowedState == ButtonsState.RIGHT_VISIBLE) dX = Math.Min(dX, -buttonWidth);
                    base.OnChildDraw(c, recyclerView, viewHolder, dX, dY, actionState, isCurrentlyActive);
                }
                else
                {
                    setTouchListener(c, recyclerView, viewHolder, dX, dY, actionState, isCurrentlyActive);
                }
            }

            if (buttonShowedState == ButtonsState.GONE)
            {
                base.OnChildDraw(c, recyclerView, viewHolder, dX, dY, actionState, isCurrentlyActive);
            }

            currentItemViewHolder = viewHolder;
        }



        //private override void SetTouchListener(final Canvas c, final RecyclerView recyclerView, final RecyclerView.
        //    ViewHolder viewHolder, final float dX, final float dY, final int actionState, final boolean
        //    isCurrentlyActive)
        //{
        //    recyclerView.setOnTouchListener(new View.OnTouchListener()
        //    {
        //        @Override
        //        public boolean onTouch(View v, MotionEvent event)
        //        {
        //        swipeBack =
        //        event.getAction() == MotionEvent.ACTION_CANCEL || event.getAction() == MotionEvent.ACTION_UP;
        //        if (swipeBack)
        //        {
        //        if (dX < -buttonWidth) buttonShowedState = ButtonsState.RIGHT_VISIBLE;
        //        else if (dX > buttonWidth) buttonShowedState = ButtonsState.LEFT_VISIBLE;

        //        if (buttonShowedState != ButtonsState.GONE)
        //        {
        //        setTouchDownListener(c, recyclerView, viewHolder, dX, dY, actionState, isCurrentlyActive);
        //        setItemsClickable(recyclerView, false);
        //    }
        //    }
        //    return false;
        //    }
        //    });
        //}

        //private void setTouchDownListener(final Canvas c, final RecyclerView recyclerView, final RecyclerView.
        //    ViewHolder viewHolder, final float dX, final float dY, final int actionState, final boolean
        //    isCurrentlyActive)
        //{
        //    recyclerView.setOnTouchListener(new View.OnTouchListener()
        //    {
        //        @Override
        //        public boolean onTouch(View v, MotionEvent event)
        //        {
        //        if (event.getAction() == MotionEvent.ACTION_DOWN) {
        //        setTouchUpListener(c, recyclerView, viewHolder, dX, dY, actionState, isCurrentlyActive);
        //    }
        //    return false;
        //    }
        //    });
        //}

        //private void setTouchUpListener(final Canvas c, final RecyclerView recyclerView, final RecyclerView.
        //    ViewHolder viewHolder, final float dX, final float dY, final int actionState, final boolean
        //    isCurrentlyActive)
        //{
        //    recyclerView.setOnTouchListener(new View.OnTouchListener()
        //    {
        //        @Override
        //        public boolean onTouch(View v, MotionEvent event)
        //        {
        //        if (event.getAction() == MotionEvent.ACTION_UP) {
        //        SwipeController.super.onChildDraw(c, recyclerView, viewHolder, 0F, dY, actionState, isCurrentlyActive);
        //        recyclerView.setOnTouchListener(new View.OnTouchListener() {
        //        @Override
        //        public boolean onTouch(View v, MotionEvent event)
        //        {
        //        return false;
        //    }
        //    });
        //    setItemsClickable(recyclerView, true);
        //    swipeBack = false;

        //    if (buttonsActions != null && buttonInstance != null && buttonInstance.contains(event.getX(), event.getY()))
        //    {
        //        if (buttonShowedState == ButtonsState.LEFT_VISIBLE)
        //        {
        //            buttonsActions.onLeftClicked(viewHolder.getAdapterPosition());
        //        }
        //        else if (buttonShowedState == ButtonsState.RIGHT_VISIBLE)
        //        {
        //            buttonsActions.onRightClicked(viewHolder.getAdapterPosition());
        //        }
        //    }
        //    buttonShowedState = ButtonsState.GONE;
        //    currentItemViewHolder = null;
        //    }
        //    return false;
        //    }
        //    });
        //}

        //private void setItemsClickable(RecyclerView recyclerView, bool isClickable)
        //{
        //    for (int i = 0; i < recyclerView.ChildCount; ++i)
        //    {
        //        recyclerView.GetChildAt(i).Clickable = isClickable;
        //    }
        //}

        //private void drawButtons(Canvas c, RecyclerView.ViewHolder viewHolder)
        //{
        //    float buttonWidthWithoutPadding = buttonWidth - 20;
        //    float corners = 16;

        //    View itemView = viewHolder.ItemView;
        //    Paint p = new Paint();

        //    RectF leftButton = new RectF(itemView.Left, itemView.Top,
        //        itemView.Left + buttonWidthWithoutPadding, itemView.Bottom);
        //    p.Color = Color.Blue;
        //    c.DrawRoundRect(leftButton, corners, corners, p);

        //    DrawText("EDIT", c, leftButton, p);

        //    RectF rightButton = new RectF(itemView.Right - buttonWidthWithoutPadding, itemView.Top,
        //        itemView.Right, itemView.Bottom);
        //    p.Color = (Color.Red);
        //    c.DrawRoundRect(rightButton, corners, corners, p);

        //    DrawText("DELETE", c, rightButton, p);

        //    buttonInstance = null;
        //    if (buttonShowedState == ButtonsState.LEFT_VISIBLE)
        //    {
        //        buttonInstance = leftButton;
        //    }
        //    else if (buttonShowedState == ButtonsState.RIGHT_VISIBLE)
        //    {
        //        buttonInstance = rightButton;
        //    }
        //}

        //private void DrawText(String text, Canvas c, RectF button, Paint p)
        //{
        //    float textSize = 60;
        //    p.Color = (Color.White);
        //    p.AntiAlias = (true);
        //    p.TextSize = (textSize);

        //    float textWidth = p.MeasureText(text);
        //    c.DrawText(text, button.CenterX() - (textWidth / 2), button.CenterY() + (textSize / 2), p);
        //}

        //public void onDraw(Canvas c)
        //{
        //    if (currentItemViewHolder != null)
        //    {
        //        drawButtons(c, currentItemViewHolder);
        //    }
        //}

    }
}