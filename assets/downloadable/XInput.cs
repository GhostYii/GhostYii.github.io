//ORG: Ghostyii & MoonLight Game
using XInputDotNetPure;

static public class XInput
{
    static private bool[,] buttonPressed = new bool[4, 14];
    static private bool[,] buttonUp = new bool[4, 14];
    static private bool[,] axisPressed = new bool[4, 6];
    static private bool[,] axisUp = new bool[4, 6]; 

    static public bool HasConnected(PlayerIndex index)
    {
        return GamePad.GetState(index).IsConnected;
    }

    static public int GetConnectedPadNum()
    {
        int num = 0;

        for (int i = 0; i < 4; i++)
            if (GamePad.GetState((PlayerIndex)i).IsConnected)
                num++;

        return num;
    }

    static public bool GetButtonDown(PlayerIndex index, XboxButton button)
    {
        if (HasConnected(index))
        {
            if (!buttonPressed[(int)index, (int)button] && button.ToButtonState(index) == ButtonState.Pressed)
            {
                buttonPressed[(int)index, (int)button] = true;
                return true;
            }
            else if (buttonPressed[(int)index, (int)button] && button.ToButtonState(index) == ButtonState.Released)
            {
                buttonPressed[(int)index, (int)button] = false;
                return false;
            }
            else
                return false;
        }
        else
            return false;
    }
    static public bool GetButton(PlayerIndex index, XboxButton button)
    {
        if (HasConnected(index))
            return button.ToButtonState(index) == ButtonState.Pressed;
        else
            return false;
    }
    static public bool GetButtonUp(PlayerIndex index, XboxButton button)
    {
        if (HasConnected(index))
        {
            if (!buttonUp[(int)index, (int)button] && button.ToButtonState(index) == ButtonState.Pressed)
            {
                buttonUp[(int)index, (int)button] = true;
                return false;
            }
            else if (buttonUp[(int)index, (int)button] && button.ToButtonState(index) == ButtonState.Released)
            {
                buttonUp[(int)index, (int)button] = false;
                return true;
            }
            else
                return false;
        }
        else
            return false;
    }

    static public float GetAxis(PlayerIndex index, XboxAxis axis)
    {
        if (HasConnected(index))
        {
            GamePadState gps = GamePad.GetState(index);
            switch (axis)
            {
                case XboxAxis.LX:
                    return gps.ThumbSticks.Left.X;
                case XboxAxis.LY:
                    return gps.ThumbSticks.Left.Y;
                case XboxAxis.RX:
                    return gps.ThumbSticks.Right.X;
                case XboxAxis.RY:
                    return gps.ThumbSticks.Right.X;
                case XboxAxis.LT:
                    return gps.Triggers.Left;
                case XboxAxis.RT:
                    return gps.Triggers.Right;
                default:
                    return 0f;
            }
        }
        else
            return 0f;
    }

    static public bool GetAxisAsButton(PlayerIndex index, XboxAxis axis, float cmpBase = 0, CompareType cmp = CompareType.Greater)
    {
        if (HasConnected(index))
            return GetCmpResult(GetAxis(index, axis), cmpBase, cmp);
        else
            return false;
    }
    static public bool GetAxisAsButtonDown(PlayerIndex index, XboxAxis axis, float cmpBase = 0, CompareType cmp = CompareType.Greater)
    {
        if (HasConnected(index))
        {
            if (!axisPressed[(int)index, (int)axis] && axis.ToButtonState(index) == ButtonState.Pressed)
            {
                axisPressed[(int)index, (int)axis] = true;
                return true;
            }
            else if (axisPressed[(int)index, (int)axis] && axis.ToButtonState(index) == ButtonState.Released)
            {
                axisPressed[(int)index, (int)axis] = false;
                return false;
            }
            else
                return false;
        }
        else return false;
    }

    static public void SetLeftVibration(PlayerIndex index, float strength)
    { GamePad.SetVibration(index, strength, 0.01f); }
    static public void SetRightVibration(PlayerIndex index, float strength)
    { GamePad.SetVibration(index, 0.01f, strength); }
    static public void SetVibration(PlayerIndex index, float leftMotor, float rightMotor)
    { GamePad.SetVibration(index, leftMotor, rightMotor); }

    static public void StopVibration(PlayerIndex index)
    { SetVibration(index, 0, 0); }

    static public ButtonState NameToButton(this GamePadButtons gpbs, string name)
    {
        name = name.ToUpper();
        switch (name)
        {
            case "A":
                return gpbs.A;
            case "B":
                return gpbs.B;
            case "X":
                return gpbs.X;
            case "Y":
                return gpbs.Y;
            case "LB":
                return gpbs.LeftShoulder;
            case "RB":
                return gpbs.RightShoulder;
            case "L":
                return gpbs.LeftStick;
            case "R":
                return gpbs.RightStick;
            case "START":
                return gpbs.Start;
            case "MENU":
                return gpbs.Guide;
            default:
                return ButtonState.Released;
        }
    }
    static public ButtonState ToButtonState(this XboxButton xb, PlayerIndex index)
    {
        GamePadButtons gpbs = GamePad.GetState(index).Buttons;
        GamePadDPad gdpd = GamePad.GetState(index).DPad;

        switch (xb)
        {
            case XboxButton.X:
                return gpbs.X;
            case XboxButton.Y:
                return gpbs.Y;
            case XboxButton.A:
                return gpbs.A;
            case XboxButton.B:
                return gpbs.B;
            case XboxButton.Start:
                return gpbs.Start;
            case XboxButton.Menu:
                return gpbs.Guide;
            case XboxButton.L:
                return gpbs.LeftStick;
            case XboxButton.R:
                return gpbs.RightStick;
            case XboxButton.LB:
                return gpbs.LeftShoulder;
            case XboxButton.RB:
                return gpbs.RightShoulder;
            case XboxButton.Up:
                return gdpd.Up;
            case XboxButton.Down:
                return gdpd.Down;
            case XboxButton.Left:
                return gdpd.Left;
            case XboxButton.Right:
                return gdpd.Right;
            default:
                return ButtonState.Released;
        }
    }
    static public ButtonState ToButtonState(this XboxAxis xa, PlayerIndex index, float cmpBase = 0, CompareType cmp = CompareType.Greater)
    {
        return GetCmpResult(GetAxis(index, xa), cmpBase, cmp) ? ButtonState.Pressed : ButtonState.Released;
    }

    static private bool GetCmpResult(float value, float cmpBase, CompareType cmp)
    {
        switch (cmp)
        {
            case CompareType.Greater:
                return value > cmpBase;
            case CompareType.GreaterOrEqual:
                return value >= cmpBase;
            case CompareType.Less:
                return value < cmpBase;
            case CompareType.LessOrEqual:
                return value <= cmpBase;
            case CompareType.Equal:
                return value == cmpBase;
            default:
                return false;
        }
    }
}

public enum XboxButton
{
    X,
    Y,
    A,
    B,
    Start,
    Menu,
    L,
    R,
    LB,
    RB,
    Up,
    Down,
    Left,
    Right
}
public enum XboxAxis
{
    LX,
    LY,
    RX,
    RY,
    LT,
    RT
}
public enum CompareType
{
    Greater,
    GreaterOrEqual,
    Less,
    LessOrEqual,
    Equal
}
