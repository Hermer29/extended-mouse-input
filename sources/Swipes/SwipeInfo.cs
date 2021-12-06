namespace Rubicks.ExtendedPointerInput
{
    public struct SwipeInfo
    {
        public readonly float length;
        public readonly SwipeDirection direction;
        public readonly int swipeId;

        public SwipeInfo(int swipeId, float length, SwipeDirection direction)
        {
            this.length = length;
            this.direction = direction;
            this.swipeId = swipeId;
        }
    }
}
