namespace OverGraphed
{
    public delegate void Event(object sender);
    public delegate void Event<in TEventArgs>(object sender, TEventArgs e);
}