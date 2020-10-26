package pixelGeometry;

import java.awt.Color;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.RenderingHints;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;
import java.util.Random;

import javax.imageio.ImageIO;
import javax.swing.JFrame;
import javax.swing.JPanel;

//                   some methods to feed or dig a boolean matrice with some geometrical                     //
//                   simple figures (rectangle, circle, line), with a bonus gallery mining ;)                //

public class PixelGeometryGenerator extends JPanel {
	public boolean[][] PixelArray = new boolean [2000][1000];

	public void paint(Graphics g) {
		Graphics2D g2 = (Graphics2D) g;
		g2.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON);
		super.paintComponents(g2);

		// initialize PixelArray
		for (int fx=0; fx<PixelArray.length; fx++) {
			for (int fy=0; fy<PixelArray[0].length; fy++) {
				PixelArray[fx][fy] = false;
			}
		}

		// feeding PixelArray
		int seed = 42;

		// testing it to make a simple moutain with mines
		PixelChaoticPente(0, 0, 1000, 999, seed);
		PixelChaoticPente(1000, 999, 2000, 0, seed);
		PixelGalleryMining(seed);

		// showing PixelArray in a graphical matrice
		for (int fx=0; fx<PixelArray.length; fx++) {
			for (int fy=0; fy<PixelArray[0].length; fy++) {
				if (PixelArray[fx][fy] == true) {
					g2.fillRect(fx, fy, 1, 1);
				}
			}
		}

		try {imageIoWrite("save");} catch (IOException e) {}
	}

	// the method to feed a matrice with a rectangle
	public void PixelRect(int x, int y, int Width, int Height, boolean fill) {
		for (int fx=x; fx<Width+x; fx++) {
			if (fx>=0 && fx<=PixelArray.length-1) {
				for (int fy=y; fy<Height+y; fy++) {
					if (fy >=0 && fy<=PixelArray[0].length-1)
						PixelArray[fx][fy] = fill;
				}
			}
		}
	}

	// the method to feed a matrice with a circle
	public void PixelCircle(int x, int y, int Rayon) {
		int D;
		double C;
		int segment;

		for (int step=0; step<=Rayon; step++) {
			D = Rayon-step;
			C = Math.sqrt(Math.pow(Rayon,2)-Math.pow(D,2));
			segment = (int) Math.floor(C);

			if (x+step>=0 && x+step<=PixelArray[0].length-1) {
				for (int s=0; s<segment; s++)
					if (y+s >=0 && y+s<=PixelArray[0].length-1)
						PixelArray[x+step][y+s]=true;
				for (int s=0; s<segment; s++)
					if (y-s >=0 && y-s<=PixelArray[0].length-1)
						PixelArray[x+step][y-s]=true;
			}
		}

		for (int step=Rayon; step<=Rayon*2; step++) {
			D = Rayon-step;
			C = Math.sqrt(Math.pow(Rayon,2)-Math.pow(D,2));
			segment = (int) Math.floor(C);

			if (x+step>=0 && x+step<=PixelArray.length) {
				for (int s=0; s<segment; s++)
					if (y+s >=0 && y+s<=PixelArray[0].length)
						PixelArray[x+step][y+s]=true;
				for (int s=0; s<segment; s++)
					if (y-s >=0 && y-s<=PixelArray[0].length)
						PixelArray[x+step][y-s]=true;
			}
		}
	}

	// the method to feed a matrice with an angular line
	public void PixelAngularLine(int x1, int y1, int width, int angle, boolean fill) {
		// calculer l'angle YL/XL
		double angleRadian=(angle*(Math.PI/180));
		int Ypoint=0;

		if (angle<0 || angle>360)
			System.out.println("angle must be beetween 0 & 360 degrees");

		if (angle==90) {
			int YL = width;
			for (int X=0; X<YL; X++) {
				if (x1+X>0 && x1+X<1000) {
					if (y1+Ypoint>0 && y1+Ypoint<1000) {
						PixelArray[x1][y1+X] = fill;
					}
				}
			}			
		}

		if (angle==270) {
			int YL = width;
			for (int X=0; X<Math.abs(YL); X++) {
				if (x1-X>=0 && x1-X<1000) {
					if (y1+Ypoint>0 && y1+Ypoint<1000) {
						PixelArray[x1][y1-X] = fill;
					}
				}
			}
		}

		if (angle>=0 && angle<90) {
			int YL = (int) (Math.abs(Math.cos(angleRadian)*width));

			for (int X=0; X<YL; X++) {
				if (x1+X>0 && x1+X<1000) {
					Ypoint = (int) Math.abs((Math.tan(angleRadian)*X));
					if (y1+Ypoint>0 && y1+Ypoint<1000) {
						PixelArray[x1+X][y1+Ypoint] = fill;
					}
				}
			}
		}

		if (angle >=90 && angle<=180) {
			int YL = (int) (Math.abs(Math.cos(angleRadian)*width));

			for (int X=YL; X>0; X--) {
				if (x1-X>0 && x1-X<1000) {
					Ypoint = (int) Math.abs((Math.tan(angleRadian)*X));
					if (y1+Ypoint>0 && y1+Ypoint<1000) {
						PixelArray[x1-X][y1+(int)Ypoint] = fill;
					}
				}
			}			
		}

		if (angle>180 && angle<=270) {
			int YL = (int) (Math.cos(angleRadian)*width);

			for (int X=Math.abs(YL); X>0; X--) {
				if (x1-X>0 && x1-X<1000) {
					Ypoint = (int) Math.abs((Math.tan(angleRadian)*X));
					if (y1+Ypoint>0 && y1+Ypoint<1000) {
						PixelArray[x1-X][y1-Ypoint] = fill;
					}
				}
			}
		}		

		if (angle>270 && angle<=360) {
			int YL = (int) Math.abs((Math.cos(angleRadian)*width));

			for (int X=0; X<Math.abs(YL); X++) {
				if (x1+X>0 && x1+X<1000) {
					Ypoint = (int) (Math.abs(Math.tan(angleRadian)*X));
					if (y1+Ypoint>0 && y1+Ypoint<1000) {
						PixelArray[x1+X][y1-Ypoint] = fill;
					}
				}
			}	
		}	
	}

	// the method to feed a matrice with a straight line
	public void PixelLine(int x1, int y1, int x2, int y2, int epaisseur, boolean fill) {
		int XL=Math.abs(x2-x1);
		int YL=y2-y1;
		double Ypoint=0;

		// calculer l'angle YL/XL
		double angle = Math.atan(((double) Math.abs(YL))/XL);
		int step = (int) Math.floor((Math.tan(angle)*1));

		if (YL<0) {
			for (int X=0; X<XL; X++) {
				if (x1+X+epaisseur >=0 && x1+X+epaisseur<=PixelArray.length-1) {
					if (y1-(int)Ypoint-step >=0 && y1-(int)Ypoint<=PixelArray[0].length-step) {
						Ypoint = Math.floor((Math.tan(angle)*X));

						for (int e=0; e<epaisseur; e++)
							for (int s=0; s<step; s++)
								PixelArray[x1+X+e][y1-(int)Ypoint-s] = fill;
					}
				}
			}
		}
		if (YL>=0) {
			for (int X=0; X<XL; X++) {
				if (x1+X+epaisseur >=0 && x1+X+epaisseur<=PixelArray.length-1) {
					if (y1+(int)Ypoint+step >=0 && y1+(int)Ypoint<=PixelArray[0].length+step) {
						Ypoint = Math.floor((Math.tan(angle)*X));

						for (int e=0; e<epaisseur; e++)
							for (int s=0; s<step; s++)
								PixelArray[x1+X+e][y1+(int)Ypoint+s] = fill;
					}
				}
			}
		}
	}

	// the method to feed a matrice with a random filled incline
	public void PixelChaoticPente(int x1, int y1, int x2, int y2, long seed) {
		int XL=Math.abs(x2-x1);
		int YL=y2-y1;
		double Ypoint=0;

		int epaisseur=10;
		Random Yset = new Random();
		Yset.setSeed(seed);
		int RandNum = 0;

		// calculer l'angle YL/XL
		double angle = Math.atan(((double) Math.abs(YL))/XL);

		for (int X=0; X<XL; X++) {
			RandNum=Yset.nextInt(epaisseur);
			Ypoint = Math.floor((Math.tan(angle)*X));

			if (YL>=0) {
				for (int y=y2; y > y2-(int) ((Ypoint-RandNum)) ; y--) {
					PixelArray[x1+X][y]=true;
				}
			}

			if (YL<0) {
				for (int y=y1; y > (y2+(int)(Ypoint-RandNum))+epaisseur ; y--) {
					PixelArray[x1+X][y] = true;
				}
			}
		}
	}

	// the method to dig a matrice with a serie of mostly random cross
	public void PixelGalleryMining(int seed) {
		Random MineRand = new Random();
		MineRand.setSeed(seed);
		int x;
		int y;
		int length;
		float roundFloat;

		// lets dig 50 cross - a good balance for a lot of paths possibilities
		for(int s=0; s<50; s++) {
			length=50+MineRand.nextInt(300);
			x=length+MineRand.nextInt(PixelArray.length-length);
			y=length+MineRand.nextInt(PixelArray[0].length-length);

			// we round the values of the cross to obtain a most coherent
			// network with potentialy more paths to walkthrough
			roundFloat = x/100;
			x = (int) roundFloat*100;
			roundFloat = y/100;
			y = (int) roundFloat*100;
			roundFloat = length/10;
			length = (int) roundFloat*10;

			PixelRect(x-(length/2), y, length, 4, false);
			PixelRect(x, y-(length/2), 4, length, false);
		}
	}

	public void imageIoWrite(String filename) throws IOException {
		BufferedImage image = new BufferedImage(PixelArray.length, PixelArray[0].length, BufferedImage.TYPE_INT_ARGB);
		Graphics2D g2 = image.createGraphics();
		g2.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON);

		g2.setColor(Color.white);
		g2.fillRect(0, 0, PixelArray.length, PixelArray[0].length);
		g2.setColor(Color.black);

		// showing PixelArray in a graphical matrice
		for (int fx=0; fx<PixelArray.length; fx++) {
			for (int fy=0; fy<PixelArray[0].length; fy++) {
				if (PixelArray[fx][fy] == true) {
					g2.fillRect(fx, fy, 1, 1);
				}
			}
		}

		try { ImageIO.write(image, "png", new File(filename+".png")); } catch (IOException e) {}
	}

	public static void main(String[] args) {
		JFrame f = new JFrame();
		f.setDefaultCloseOperation(JFrame.DISPOSE_ON_CLOSE);
		f.getContentPane().add(new PixelGeometryGenerator());
		f.setSize(2000, 1000);
		f.setLocationRelativeTo(null);
		f.setVisible(true);
	}
}
