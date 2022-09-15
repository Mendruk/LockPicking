using System.Media;

namespace Lock_Picking;

public class Game
{
    public static Random Random = new();

    private static readonly Font font = new(FontFamily.GenericSansSerif, 12, FontStyle.Bold);
    private static readonly StringFormat Format = new();

    private readonly Bitmap lockBitmap = Resource.Lock;
    private readonly Bitmap keyHoleBitmap = Resource.Keyhole;
    private readonly Bitmap lockpickBitmap = Resource.Lockpick;
    private readonly Bitmap screwdriverBitmap = Resource.Screwdriver;

    private readonly SoundPlayer lockPickingEnter = new(Resource.lockpicking_enter);
    private readonly SoundPlayer lockPickingFail = new(Resource.lockpicking_fail);
    private readonly SoundPlayer lockPickingSuccess = new(Resource.lockpicking_unlock);
    private readonly Bitmap lockpickTakeDamage = Resource.LockpickTakeDamage;

    private readonly int width;
    private readonly int height;

    private readonly int incompleteOpeningZoneBoundary = 30;
    private readonly int openingZoneBoundary = 5;
    private readonly double screwdriverRotationStep = 4;


    private double screwdriverAngle;
    private double screwdriverAngleLimit;
    public double targetAngle;

    private int brakingLockpicks;
    private int breakingLock;
    private int strength;

    private bool isLockPickTakeDamage;

    public bool isKeyPressed;
    public double lockpickAngle;

    public Game(int gameFiledWidth, int gameFieldHeight)
    {
        width = gameFiledWidth;
        height = gameFieldHeight;

        Format.Alignment = StringAlignment.Center;
        strength = 10;
        targetAngle = GetRandomTargetAngle();
        Restart();
        lockPickingEnter.Play();
    }

    public void Update()
    {
        screwdriverAngleLimit = GetScrewdriverAngleLimit();

        if (isKeyPressed)
        {
            if (screwdriverAngle < screwdriverAngleLimit)
            {
                screwdriverAngle += screwdriverRotationStep;

                if (screwdriverAngle >= 90)
                {
                    lockPickingSuccess.Play();
                    breakingLock++;
                    Restart();
                    targetAngle = GetRandomTargetAngle();
                }
            }
            else
            {
                isLockPickTakeDamage = true;
                if (strength > 1)
                {
                    strength--;
                }
                else
                {
                    lockPickingFail.Play();
                    brakingLockpicks++;
                    Restart();
                    strength = 10;
                }
            }
        }
        else
        {
            isLockPickTakeDamage = false;
            if (screwdriverAngle > 0)
                screwdriverAngle -= screwdriverRotationStep;
        }
    }

    //TODO
    public void Draw(Graphics graphics)
    {
        string indicatorText = $"Взломанные замки:{breakingLock}" +
                               $"\nСломанные отмычки:{brakingLockpicks}";

        graphics.DrawString(indicatorText, font, Brushes.White, width / 2, height / 20, Format);

        graphics.TranslateTransform(width / 2, height / 2);

        graphics.DrawImage(lockBitmap, -lockBitmap.Width / 2 - 1, -lockBitmap.Height / 2 + 3);

        graphics.RotateTransform((float)screwdriverAngle);

        graphics.DrawImage(keyHoleBitmap, -keyHoleBitmap.Width / 2, -keyHoleBitmap.Height / 2);
        graphics.DrawImage(screwdriverBitmap, 0, -screwdriverBitmap.Height);

        graphics.ResetTransform();

        int lockpickX = (int)(20 * Math.Cos((screwdriverAngle - 90) * Math.PI / 180));
        int lockpickY = (int)(20 * Math.Sin((screwdriverAngle - 90) * Math.PI / 180));

        graphics.TranslateTransform(width / 2 + lockpickX, height / 2 + lockpickY);
        graphics.RotateTransform((float)lockpickAngle + 90);

        if (!isLockPickTakeDamage)
            graphics.DrawImage(lockpickBitmap, -lockpickBitmap.Width / 2, 0);
        else
            graphics.DrawImage(lockpickTakeDamage, -lockpickTakeDamage.Width / 2, 0);

        graphics.ResetTransform();
    }

    private void Restart()
    {
        isKeyPressed = false;
        screwdriverAngle = 0;
        lockpickAngle = 90;
    }

    private double GetScrewdriverAngleLimit()
    {
        if (lockpickAngle < targetAngle + openingZoneBoundary &&
            lockpickAngle > targetAngle - openingZoneBoundary)
            return 90;

        if (lockpickAngle < targetAngle + incompleteOpeningZoneBoundary &&
            lockpickAngle > targetAngle - incompleteOpeningZoneBoundary)
            return (1 - (Math.Abs(-lockpickAngle + targetAngle)) / incompleteOpeningZoneBoundary) * 90;

        return 0;
    }

    private double GetRandomTargetAngle()
    {
        return Random.Next(0, 180); //todo;
    }
}