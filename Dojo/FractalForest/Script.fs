﻿open System
open System.Drawing
open System.Windows.Forms

module FractalForest =

    // Create a form to display the graphics
    let width, height = 500, 500         
    let form = new Form(Width = width, Height = height)
    let box = new PictureBox(BackColor = Color.White, Dock = DockStyle.Fill)
    let image = new Bitmap(width, height)
    let graphics = Graphics.FromImage(image)
    //The following line produces higher quality images, 
    //at the expense of speed. Uncomment it if you want
    //more beautiful images, even if it's slower.
    //Thanks to https://twitter.com/AlexKozhemiakin for the tip!
    graphics.SmoothingMode <- System.Drawing.Drawing2D.SmoothingMode.HighQuality
    let brush = new SolidBrush(Color.Brown)

    box.Image <- image
    form.Controls.Add(box) 

    // Compute the endpoint of a line
    // starting at x, y, going at a certain angle
    // for a certain length. 
    let endpoint x y angle length =
        x + length * cos angle,
        y + length * sin angle

    let flip x = (float)height - x

    // Utility function: draw a line of given width, 
    // starting from x, y
    // going at a certain angle, for a certain length.
    let drawLine (target : Graphics) (brush : Brush) 
                 (x : float) (y : float) 
                 (angle : float) (length : float) (width : float) =
        let x_end, y_end = endpoint x y angle length
        let origin = new PointF((single)x, (single)(y |> flip))
        let destination = new PointF((single)x_end, (single)(y_end |> flip))
        let pen = new Pen(brush, (single)width)
        target.DrawLine(pen, origin, destination)

    let draw x y angle length width brush = 
        drawLine graphics brush x y angle length width

    let pi = Math.PI

    type Branch = Left | Right

    let nextBrush step branch brush =
        match step % 2, branch with
        | 1, Right -> new SolidBrush(Color.DarkGreen)
        | 0, Left -> new SolidBrush(Color.DarkGoldenrod)
        | _ -> brush

    // Now... your turn to draw
    // The trunk
//    draw 250. 50. (pi*(0.5)) 100. 4.
//    let x, y = endpoint 250. 50. (pi*(0.5)) 100.
    // first and second branches
//    draw x y (pi*(0.5 + 0.3)) 50. 2.
//    draw x y (pi*(0.5 - 0.4)) 50. 2.

    let rec drawFractal x y angle length width step brush =
         match step with
         | 20 -> ()
         | _ -> 
            draw x y (pi*0.5 + angle) length width brush
            let x1, y1 = endpoint x y (pi*0.5 + angle) length
            drawFractal x1 y1 (angle + 0.1) (length/1.1) width (step + 1) (nextBrush step Left brush)
            drawFractal x1 y1 (angle - 0.1) (length/1.1) width (step + 1) (nextBrush step Right brush)      


    // left tree
    drawFractal 100. 10. 0. 50. 2. 0 brush        
    // center tree
    drawFractal 250. 10. 0. 50. 2. 0 brush        
    // right tree
    drawFractal 400. 10. 0. 50. 2. 0 brush        

    ignore (form.ShowDialog())

    (* To do a nice fractal tree, using recursion is
    probably a good idea. The following link might
    come in handy if you have never used recursion in F#:
    http://en.wikibooks.org/wiki/F_Sharp_Programming/Recursion
    *)