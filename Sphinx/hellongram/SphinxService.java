package edu.cmu.sphinx.demo.hellongram;

import java.io.IOException;
import java.io.PrintWriter;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.net.URL;


import edu.cmu.sphinx.frontend.util.AudioFileDataSource;
import edu.cmu.sphinx.recognizer.Recognizer;
import edu.cmu.sphinx.result.Result;
import edu.cmu.sphinx.util.props.ConfigurationManager;



public class SphinxService extends HttpServlet {
	private static final long serialVersionUID = 1L;
       

    public SphinxService() {
        super();
        // TODO Auto-generated constructor stub
    }
    protected void doGet(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
    	ConfigurationManager cm;
		URL audioURL;
		audioURL = SphinxService.class.getResource("test3.wav");

		cm = new ConfigurationManager(SphinxService.class.getResource("hellongram.config.xml"));
		
		Recognizer recognizer = (Recognizer) cm.lookup("recognizer");
		recognizer.allocate();

		AudioFileDataSource dataSource = (AudioFileDataSource) cm.lookup("audioFileDataSource");
		dataSource.setAudioFile(audioURL, null);

		Result result;
		String resultText = null ;
		while ((result = recognizer.recognize())!= null) {
			resultText = result.getBestResultNoFiller();
		}
		
		response.setContentType("text/html");

	    PrintWriter out = response.getWriter();

	    out.println("<html><head><title>My Servlet</title></head><body>");
	    out.println(resultText);
	    out.println("<body></html>");

	    out.close();
    }


	protected void doPost(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		
	}

}
