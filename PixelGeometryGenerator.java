import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.RenderingHints;
import javax.swing.JFrame;
import javax.swing.JPanel;

public class PixelGeometryGenerator extends JPanel {
	public boolean[][] PixelArray = new boolean [100][100];
	public void paint(Graphics g) {
		Graphics2D g2 = (Graphics2D) g;
		g2.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON);
		super.paintComponents(g2);
		
		// initialize PixelArray
		for (int fx=0; fx<100; fx++) {
			for (int fy=0; fy<100; fy++) {
				PixelArray[fx][fy] = false;
			}
		}

		// feeding PixelArray
		PixelCircle(30, 30, 90);
		PixelCircle(20, 10, 20);
		
		// showing PixelArray in a graphical matrice
		for (int fx=0; fx<100; fx++) {
			for (int fy=0; fy<100; fy++) {
				if (PixelArray[fx][fy] == true) {
					g2.fillRect(fx*10, fy*10, 10, 10);
				}
				if (PixelArray[fx][fy] == false) {
					g2.drawRect(fx*10, fy*10, 10, 10);
				}
			}
		}
	}
	
	// the method for feeding a matrice with a square
	public void PixelRect(int x, int y, int Width, int Height) {
		for (int fx=x; fx<Width+x; fx++) {
			for (int fy=y; fy<Height+y; fy++) {
				PixelArray[fx][fy] = true;
			}
		}
	}

	// the method for feeding a matrice with a circle
	public void PixelCircle(int x, int y, int Rayon) {
		int D;
		double C;
		int segment;

		for (int step=0; step<=Rayon; step++) {
			D = Rayon-step;
			C = Math.sqrt(Math.pow(Rayon,2)-Math.pow(D,2));
			segment = (int) Math.floor(C);

			if (x+step>=0 && x+step<=99) {
				for (int s=0; s<segment; s++)
					if (y+s >=0 && y+s<=99)
						PixelArray[x+step][y+s]=true;
				for (int s=0; s<segment; s++)
					if (y-s >=0 && y-s<=99)
						PixelArray[x+step][y-s]=true;
			}
		}

		for (int step=Rayon; step<=Rayon*2; step++) {
			D = Rayon-step;
			C = Math.sqrt(Math.pow(Rayon,2)-Math.pow(D,2));
			segment = (int) Math.floor(C);

			if (x+step>=0 && x+step<=99) {
				for (int s=0; s<segment; s++)
					if (y+s >=0 && y+s<=99)
						PixelArray[x+step][y+s]=true;
				for (int s=0; s<segment; s++)
					if (y-s >=0 && y-s<=99)
						PixelArray[x+step][y-s]=true;
			}
		}
	}

	public static void main(String[] args) {
		JFrame f = new JFrame();
		f.setDefaultCloseOperation(JFrame.DISPOSE_ON_CLOSE);
		f.getContentPane().add(new PixelGeometryGenerator());
		f.setSize(1000, 1000);
		f.setLocationRelativeTo(null);
		f.setVisible(true);
	}
}
