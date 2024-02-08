namespace RPG.Control
{
    public interface IRayCastable
    {
        CursorModes GetCursorType();
        bool CanHandleRaycast(MovementController controller);
    }
}