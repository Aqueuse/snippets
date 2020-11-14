import java.awt.Color;
import java.awt.Font;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.RenderingHints;
import java.awt.geom.AffineTransform;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;
import java.util.Random;

import javax.imageio.ImageIO;

public class paintWordCloud {
	static String[][] words;
	static int CurrentImageWidth;
	static int CurrentImageHeight;
	
	public static void main(String path, int width, int height) throws IOException {
		CurrentImageWidth=width;
		CurrentImageHeight=height;
		Finalpaint(path, overlapManaged(initialPlacement(path)), width, height);
	}

	// convert data from CSV to array : add defaults coords
	// random rotate, appropriate FontSize, and compute width
	// and height for the overlap management
	public static int[][] initialPlacement(String path) throws IOException {
		words = readCSV.loadFile(path);
		int[] textSize = {0,0};
		int fontSize[] = FontSize(words);

		Random rotate = new Random();
		rotate.setSeed(40);
		int[][] coordWords = new int[6][words[0].length];
		
		// coordWords [x, y, width, height, fontSize, rotate]
		for (int i=0; i<words[0].length; i++) {
			coordWords[0][i] = 0; // initial x
			coordWords[1][i] = 0; // initial y

			coordWords[4][i] = fontSize[i]; // fontSize

			textSize = textBox(words[0][i], fontSize[i]);
			coordWords[2][i] = textSize[0]; // width
			coordWords[3][i] = textSize[1]; // height
			coordWords[5][i] = rotate.nextInt(2)*90; // rotation
		}
		return coordWords;
	}
	
	public static int[][] overlapManaged(int[][] coordWords) {
		int[][] coordWordsCorrected = coordWords;

		Random placement = new Random();
		placement.setSeed(40);

		for (int c=0; c<coordWords[0].length; c++) {
			coordWordsCorrected[0][c] = placement.nextInt(CurrentImageWidth);
			coordWordsCorrected[1][c] = placement.nextInt(CurrentImageHeight);

			for (int o=0; o<coordWords[0].length; o++) {
				if (coordWordsCorrected[0][c] < coordWords[0][o]) {
					coordWordsCorrected[0][c] = 0;
					coordWordsCorrected[1][c] = 0;
				}
			}
		}
		return coordWordsCorrected;
	}

	// determine the fontSize based on the frequency
	private static int[] FontSize(String[][] ArrayWords) {
		int[] sizeFont = new int[ArrayWords[0].length];
		int currentFreq1;
		int currentFreq2;

		int maxFreq=1;
		int minFreq=1;

		// determine the max and min frequency
		for (int i=0; i<ArrayWords[0].length; i++) {
			currentFreq1=Integer.valueOf(ArrayWords[1][i]);

			for (int l=0; l<ArrayWords[0].length; l++) {
				currentFreq2=Integer.valueOf(ArrayWords[1][l]);
				if (currentFreq2>currentFreq1 && currentFreq2>maxFreq) {
					maxFreq=currentFreq2;
				}
				if (currentFreq2<currentFreq1 && currentFreq2<minFreq) {
					minFreq=currentFreq2;
				}
			}
		}

		// calculate the font size base of the
		// min, max freq and the freq of the word
		int averageFont = (maxFreq+minFreq)/2;

		for (int f=0; f<ArrayWords[0].length; f++) {
			sizeFont[f]=averageFont*Integer.valueOf(ArrayWords[1][f])*10;
		}
		return sizeFont;
	}

	// determine the size of the text zone of one word based
	// on its chars and the fontSize
	private static int[] textBox(String word, int fontSize) {
		int[] Textsize= {0,0};

		long charNb=word.chars().count();
		Textsize[0]=(int) (charNb*(fontSize/2)); // width
		Textsize[1]=(int) (fontSize); 		 // height

		return Textsize;
	}
	
	// the effective textWriting
	private static void textWrite(String string, int FontSize,
								  int x, int y,
								  int rotate,
								  int width, int height,
								  Graphics g2) {
		
		// todo : randomize color
		Color BleuCiel = new Color(45, 142, 247);

		AffineTransform affineTransform = new AffineTransform();
		affineTransform.rotate(Math.toRadians(rotate), 0, 0);

		Font font = new Font("TimesRoman", Font.PLAIN, FontSize);
		Font rotatedFont = font.deriveFont(affineTransform);
		g2.setColor(BleuCiel);
		g2.setFont(rotatedFont);

		g2.drawString(string, x, y);
		
		// draw a text box to help calibrate the overlap management later
		if (rotate==0) g2.drawRect(x, y-height, width, height);
		if (rotate==90) g2.drawRect(x, y, width, height);
	}

	static void Finalpaint(String path, int[][] coordWords, int ImageWidth, int ImageHeight) {		
		BufferedImage image = new BufferedImage(ImageWidth, ImageHeight, BufferedImage.TYPE_INT_ARGB);
		Graphics2D g2 = image.createGraphics();
		g2.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON);

		for (int w=0; w<coordWords[0].length; w++) {
			if (coordWords[5][w]==0) {  // mot horizontal
				textWrite(words[0][w], coordWords[4][w],  // string, fontSize
						  coordWords[0][w], coordWords[1][w], // x, y
					      coordWords[5][w], // rotate
					      coordWords[2][w], coordWords[3][w], // width, height
					      g2);
			}
			if (coordWords[5][w]==90) { // mot vertical
				textWrite(words[0][w], coordWords[4][w], // string, fontSize
						  coordWords[0][w], coordWords[1][w], // x, y
						  coordWords[5][w], // rotate
						  coordWords[3][w], coordWords[2][w], // height, width ;)
						  g2);
			}			
		}
		
		try { ImageIO.write(image, "png", new File("test.png")); } catch (IOException e) {}
	}
}
