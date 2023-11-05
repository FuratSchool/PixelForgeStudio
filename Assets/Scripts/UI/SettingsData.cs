public struct SettingsData
{
    public float masterVolume;
    public float effectsVolume;
    public float backgroundVolume;
    public bool fullscreen;
    public bool invertedY;
    public bool invertedX;
    public float sensitivityY;
    public float sensitivityX;
    public int resolutionWidth;
    public int resolutionHeight;
    public string rebinds;

    public SettingsData(
        string rebinds = "",
        int resolutionWidth = 1920,
        int resolutionHeight = 1080,
        float masterVolume = 0, 
        float effectsVolume = 0, 
        float backgroundVolume = 0, 
        bool fullscreen = true, 
        bool invertedY = false, 
        bool invertedX = false,
        float sensitivityY = 1, 
        float sensitivityX = 1
        ) 
    {
        this.masterVolume = masterVolume;
        this.effectsVolume = effectsVolume;
        this.backgroundVolume = backgroundVolume;
        this.resolutionWidth = resolutionWidth;
        this.resolutionHeight = resolutionHeight;
        this.fullscreen = fullscreen;
        this.invertedY = invertedY;
        this.invertedX = invertedX;
        this.sensitivityY = sensitivityY;
        this.sensitivityX = sensitivityX;
        this.rebinds = rebinds;
    }
}
