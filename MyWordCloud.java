package automationPoleEmploi;

import java.io.IOException;
import java.util.Random;

public class MyWordCloud {

	// prend un CSV avec les mots et leur fréquence
	public MyWordCloud() throws IOException {
		Random placement = new Random();
		
		String[][] words = readCSV.loadFile("motscles.csv");
		int[] textSize = {0,0};
		int fontSize[] = FontSize(words);

		int[][] coordWords = new int[4][words[0].length];
		
		// initiale feeding of the array of words :
		// coordWords [x, y, width, height, fontSize]
		for (int i=0; i<words[0].length; i++) {
			coordWords[0][i] = 0;
			coordWords[1][i] = 0;	

			coordWords[4][i] = fontSize[i];

			textSize = text(words[0][i], fontSize[i]);
			coordWords[2][i] = textSize[0];
			coordWords[3][i] = textSize[1];
		}
		
		// place randomly one text and check if
		// it's overlap an other text
		// if so decal it and recheck
		
	}

	// determine the fontSize based on the frequency
	private int[] FontSize(String[][] ArrayWords) {
		int[] sizeFont = null;
		
		// determine the max and min frequency
		
		// calculate the font size base of the
		// min, max freq and the freq of the word
		
		return sizeFont ;
	}

	// determine the size of the text zone of one word based on its
	// letters and fontSize
	private int[] text(String string, int FontSize) {
		int[] textSize = {1,1};

		return textSize;
	}

}
