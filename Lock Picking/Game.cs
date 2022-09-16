using System.Media;

namespace Lock_Picking;

public class Game
{
    private const int InitialLockpickStrength = 10;

    private static readonly Font Font = new(FontFamily.GenericSansSerif, 12, FontStyle.Bold);
    private static readonly StringFormat Format = new();

    private static Random Random = new();

    private readonly Bitmap lockBitmap = Resource.Lock;
    private readonly Bitmap keyHoleBitmap = Resource.Keyhole;
    private readonly Bitmap lockpickBitmap = Resource.Lockpick;
    private readonly Bitmap screwdriverBitmap = Resource.Screwdriver;
    private readonly Bitmap lockpickTakeDamageBitmap = Resource.LockpickTakeDamage;

    private readonly SoundPlayer lockpickingEnterSound = new(Resource.lockpicking_enter);
    private readonly SoundPlayer lockpickingFailSound = new(Resource.lockpicking_fail);
    private readonly SoundPlayer lockpickingSuccessSound = new(Resource.lockpicking_unlock);

    private readonly int gameFieldWidth;
    private readonly int gameFieldHeight;

    private readonly int incompleteOpeningZoneBoundary = 30;
    private readonly int openingZoneBoundary = 5;
    private readonly double screwdriverRotationStep = 4;


    private double screwdriverAngle;
    private double screwdriverAngleLimit;
    private double lockpickTargetAngle;

    private int brokenLockpicks;
    private int openedLocks;
    private int lockpickStrength;

    private bool isLockpickTakeDamage;

    public bool IsKeyPressed;
    public double LockpickAngle;

    public Game(int gameFieldWidth, int gameFieldHeight)
    {
        this.gameFieldWidth = gameFieldWidth;
        this.gameFieldHeight = gameFieldHeight;

        Format.Alignment = StringAlignment.Center;
        lockpickStrength = InitialLockpickStrength;
        lockpickTargetAngle = GetRandomTargetAngle();
        ResetLockpickingProcess();
        lockpickingEnterSound.Play();
    }

    public void Update()
    {
        screwdriverAngleLimit = GetScrewdriverAngleLimit();

        if (IsKeyPressed)
        {
            if (screwdriverAngle < screwdriverAngleLimit)
            {
                screwdriverAngle += screwdriverRotationStep;

                if (screwdriverAngle >= 90)
                {
                    OnLockOpenen();
                }
            }
            else
            {
                isLockpickTakeDamage = true;
                if (lockpickStrength > 0)
                {
                    lockpickStrength--;
                }
                else
                {
                    OnLockpickBroken();
                }
            }
        }
        else
        {
            isLockpickTakeDamage = false;
            if (screwdriverAngle > 0)
                screwdriverAngle -= screwdriverRotationStep;
        }
    }

    public void Draw(Graphics graphics)
    {
        int gameFieldCenterX = gameFieldWidth / 2;
        int gameFieldCenterY = gameFieldHeight / 2;

        string indicatorText = $"Взломанные замки: {openedLocks}" +
                               $"\nСломанные отмычки: {brokenLockpicks}";

                               graphics.DrawString(indicatorText, Font, Brushes.White, gameFieldCenterX, gameFieldCenterY / 10, Format);

        graphics.TranslateTransform(gameFieldCenterX, gameFieldCenterY);

        graphics.DrawImage(lockBitmap, -lockBitmap.Width / 2 - 1, -lockBitmap.Height / 2 + 3);

        graphics.RotateTransform((float)screwdriverAngle);

        graphics.DrawImage(keyHoleBitmap, -keyHoleBitmap.Width / 2, -keyHoleBitmap.Height / 2);
        graphics.DrawImage(screwdriverBitmap, 0, -screwdriverBitmap.Height);

        graphics.ResetTransform();

        int lockpickOffsetX = (int)(20 * Math.Cos((screwdriverAngle - 90) * Math.PI / 180));
        int lockpickOffsetY = (int)(20 * Math.Sin((screwdriverAngle - 90) * Math.PI / 180));

        graphics.TranslateTransform(gameFieldCenterX + lockpickOffsetX, gameFieldCenterY + lockpickOffsetY);
        graphics.RotateTransform((float)LockpickAngle + 90);

        if (!isLockpickTakeDamage)
            graphics.DrawImage(lockpickBitmap, -lockpickBitmap.Width / 2, 0);
        else
            graphics.DrawImage(lockpickTakeDamageBitmap, -lockpickTakeDamageBitmap.Width / 2, 0);

    }

    private void OnLockOpenen()
    {
        lockpickingSuccessSound.Play();
        openedLocks++;
        ResetLockpickingProcess();
        lockpickTargetAngle = GetRandomTargetAngle();
    }
    private void OnLockpickBroken()
    {
        lockpickingFailSound.Play();
        brokenLockpicks++;
        ResetLockpickingProcess();
        lockpickStrength = InitialLockpickStrength;
    }

    private void ResetLockpickingProcess()
    {
        IsKeyPressed = false;
        screwdriverAngle = 0;
        LockpickAngle = 90;
    }

    private double GetScrewdriverAngleLimit()
    {
        if (LockpickAngle < lockpickTargetAngle + openingZoneBoundary &&
            LockpickAngle > lockpickTargetAngle - openingZoneBoundary)
            return 90;

        if (LockpickAngle < lockpickTargetAngle + incompleteOpeningZoneBoundary &&
            LockpickAngle > lockpickTargetAngle - incompleteOpeningZoneBoundary)
            return (1 - (Math.Abs(-LockpickAngle + lockpickTargetAngle)) / incompleteOpeningZoneBoundary) * 90;

        return 0;
    }

    private double GetRandomTargetAngle()
    {
        return Random.Next(0, 180); //todo;
    }
}