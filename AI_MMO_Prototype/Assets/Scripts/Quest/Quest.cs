public class Quest
{
    public string description;
    public bool isCompleted;

    public Quest(string description) {
        this.description = description;
        isCompleted = false;
    }
}
