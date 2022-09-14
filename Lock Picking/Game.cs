using System.Media;

namespace Lock_Picking;

public class Game
{
    public static Random Random = new();

    //font
    private static readonly Font font = new(FontFamily.GenericSansSerif, 12, FontStyle.Bold);
    private static readonly StringFormat Format = new();

    //images
    private readonly Bitmap lockImage = Resource.Lock;
    private readonly Bitmap keyHole = Resource.Keyhole;
    private readonly Bitmap lockpick = Resource.Lockpick;
    private readonly Bitmap screwdriver = Resource.Screwdriver;
    //sounds
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
        lockPickingEnter.Play();

        width = gameFiledWidth;
        height = gameFieldHeight;
        Format.Alignment = StringAlignment.Center;
        strength = 10;

        targetAngle = GetRandomTargetAngle();
        Restart();
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
                    lockPickingSuccess.PlaySync();
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
                    lockPickingFail.PlaySync();
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
        int X = width / 2;
        int Y = height / 2;

        graphics.DrawImage(lockImage, X - lockImage.Width / 2, Y - lockImage.Height / 2, lockImage.Width,
            lockImage.Width);

        string text1 = $"Взломанные замки:{breakingLock}" +
                       $"\nСломанные отмычки:{brakingLockpicks}";

        graphics.DrawString(text1, font, Brushes.White, width / 2, height / 20, Format);

        double keyHoleReferenceLenght = Math.Sqrt(keyHole.Width * keyHole.Width + keyHole.Height * keyHole.Height) / 2;

        Point[] destinationPointsKeyHole =
        {
            new((int)(X + 2 + keyHoleReferenceLenght * Math.Cos((screwdriverAngle - 135) * Math.PI / 180)),
                (int)(Y - 3 + keyHoleReferenceLenght * Math.Sin((screwdriverAngle - 135) * Math.PI / 180))),
            new((int)(X + 2 + keyHoleReferenceLenght * Math.Cos((screwdriverAngle - 45) * Math.PI / 180)),
                (int)(Y - 3 + keyHoleReferenceLenght * Math.Sin((screwdriverAngle - 45) * Math.PI / 180))),
            new((int)(X + 2 + keyHoleReferenceLenght * Math.Cos((screwdriverAngle + 135) * Math.PI / 180)),
                (int)(Y - 3 + keyHoleReferenceLenght * Math.Sin((screwdriverAngle + 135) * Math.PI / 180)))
        };

        graphics.DrawImage(keyHole, destinationPointsKeyHole);

        double lockPickX = X + 20 * Math.Cos((screwdriverAngle - 90) * Math.PI / 180);
        double lockPickY = Y - 5 + 20 * Math.Sin((screwdriverAngle - 90) * Math.PI / 180);

        Point[] destinationPointsLockPick =
        {
            new((int)(lockPickX - lockpick.Width / 2 * Math.Cos((lockpickAngle - 90) * Math.PI / 180)),
                (int)(lockPickY - lockpick.Width / 2 * Math.Sin((lockpickAngle - 90) * Math.PI / 180))),
            new((int)(lockPickX + lockpick.Width / 2 * Math.Cos((lockpickAngle - 90) * Math.PI / 180)),
                (int)(lockPickY + lockpick.Width / 2 * Math.Sin((lockpickAngle - 90) * Math.PI / 180))),
            new((int)(lockPickX + lockpick.Height * Math.Cos(lockpickAngle * Math.PI / 180)),
                (int)(lockPickY + lockpick.Height * Math.Sin(lockpickAngle * Math.PI / 180)))
        };

        if (!isLockPickTakeDamage)
            graphics.DrawImage(lockpick, destinationPointsLockPick);
        else
            graphics.DrawImage(lockpickTakeDamage, destinationPointsLockPick);


        Point[] destinationPointsScrewdriver =
        {
            new((int)(X - screwdriver.Width / 2 * Math.Cos((screwdriverAngle - 90) * Math.PI / 180)),
                (int)(Y - 10 - screwdriver.Width / 2 * Math.Sin((screwdriverAngle - 90) * Math.PI / 180))),
            new((int)(X + screwdriver.Width / 2 * Math.Cos((screwdriverAngle - 90) * Math.PI / 180)),
                (int)(Y - 10 + screwdriver.Width / 2 * Math.Sin((screwdriverAngle - 90) * Math.PI / 180))),
            new((int)(X + screwdriver.Height * Math.Cos(screwdriverAngle * Math.PI / 180)),
                (int)(Y - 10 + screwdriver.Height * Math.Sin(screwdriverAngle * Math.PI / 180)))
        };

        graphics.DrawImage(screwdriver, destinationPointsScrewdriver);
    }

    private void Restart()
    {
        isKeyPressed = false;
        screwdriverAngle = 0;
        lockpickAngle = -90;
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
        return Random.Next(-180 + openingZoneBoundary, -openingZoneBoundary);
    }
}