public class PictureButton : Button
{
    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.DrawImage(this.BackgroundImage, 0, 0, e.ClipRectangle.Width, e.ClipRectangle.Height);
        //e.Graphics.DrawRectangle(new Pen(Color.Black), 0, 0, this.ClientSize.Width - 1, this.ClientSize.Height - 1);

        //base.OnPaint(e);
    }
}