<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="projects_PetDeskAPI.aspx.cs" Inherits="SolutionsWeb.projects_PetDeskAPI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
	<meta http-equiv="X-UA-Compatible" content="IE=edge">
	<meta name="viewport" content="width=device-width, initial-scale=1">
    <title>projects_PetDeskAPI - Solutions Web</title>
    <!-- Bootstrap -->
	<link href="../css/bootstrap.css" rel="stylesheet">
	<link href="../css/styles.css" rel="stylesheet" type="text/css">
    <link href="../css/tablecenter.css" rel="stylesheet" type="text/css">
    <link rel="icon" type="image/vnd.microsoft.icon" href="../favicon.ico" />
    <link rel="shortcut icon" href="../favicon.ico" />

	<!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
	<!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
	<!--[if lt IE 9]>
		  <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
		  <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
		<![endif]-->
</head>
<body>
    <form id="frmprojects_PetDeskAPI" runat="server">
    <div>
    	<div class="container-fluid"><!-- #BeginLibraryItem "/library/menu_s.lbi" -->      <nav class="navbar navbar-default">
        <div class="container-fluid">
          <!-- Brand and toggle get grouped for better mobile display -->
          <div class="navbar-header">
            <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#defaultNavbar1"><span class="sr-only">Toggle navigation</span><span class="icon-bar"></span><span class="icon-bar"></span><span class="icon-bar"></span></button>
            <a class="navbar-brand" href="../landing.aspx">Solutions Web</a> </div>
            <div class="navbar-links">
           <a class="linkedin" href="https://www.linkedin.com/in/khenzel" target="_blank">
           	<img src="../images/linkedin-small.png" width="65" height="65" alt=""/> </a>
            <a class="monster" href="http://beknown.com/kevin-henzel" target="_blank"><img src="../images/monster-small.gif" width="245" height="65" alt=""/></a></div>
          <!-- Collect the nav links, forms, and other content for toggling -->
          <div class="collapse navbar-collapse" id="defaultNavbar1">
<ul class="nav navbar-nav navbar-right">
        
        <li class="dropdown"><a href="../default.aspx" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-expanded="false">My Work<span class="caret"></span></a>
          <ul class="dropdown-menu" role="menu">
            <li><a href="projects_PetDeskAPI.aspx">projects_PetDeskAPI</a></li>
            <li class="divider"></li>
            <li><a href="../resume.aspx">Resume</a></li>
            <li><a href="../degrees.aspx">Degrees</a></li>
            <li><a href="../certifications.aspx">Certifications</a></li>
            <li><a href="../references.aspx">References</a></li>
            <li class="divider"></li>
            <li><a href="https://www.linkedin.com/in/khenzel">View me on Linkedin</a></li>
            <li><a href="http://beknown.com/kevin-henzel">View me on Monster.com</a></li>
            <li class="divider"></li>
            <li><a href="../cleancode.aspx">Clean Code - Why is it Important?</a></li>
            <li class="divider"></li>
            <li><a href="../contact.aspx">Contact Me</a></li>
            <li class="divider"></li>
          </ul>
        </li>
        </ul>
            <form class="navbar-form navbar-right" role="search">
              <div class="form-group" align="right">You are logged in.<a href="../default.aspx"> Sign Out </a></div>
            </form>
          </div>
          <!-- /.navbar-collapse -->
        </div>
        <!-- /.container-fluid -->
      </nav><!-- #EndLibraryItem --><div id="carousel1" class="carousel slide" data-ride="carousel">
    <ol class="carousel-indicators">
	      <li data-target="#carousel1" data-slide-to="0" class="active"></li>
	      <li data-target="#carousel1" data-slide-to="1"></li>
	      <li data-target="#carousel1" data-slide-to="2"></li>
    </ol>
	    <div class="carousel-inner" role="listbox">
	      <div class="item active"><a href="../landing.aspx"><img src="../images/Carousel_1.jpg" alt="First slide image" width="900" height="306" class="center-block"></a>
	        <div class="carousel-caption">
	          <h3></h3>
	          <p></p>
            </div>
          </div>
	      <div class="item"><a href="https://www.linkedin.com/in/khenzel" target="_blank"><img src="../images/Carousel_2.png" alt="Second slide image" width="900" height="306" class="center-block"></a>
	        <div class="carousel-caption">
	          <h3></h3>
	          <p></p>
            </div>
          </div>
	      <div class="item"><a href="http://beknown.com/kevin-henzel" target="_blank"><img src="../images/Carousel_3.png" alt="Third slide image" width="900" height="306" class="center-block"></a>
	        <div class="carousel-caption">
	          <h3></h3>
	          <p></p>
            </div>
          </div>
        </div>
      <a class="left carousel-control" href="#carousel1" role="button" data-slide="prev"><span class="glyphicon glyphicon-chevron-left" aria-hidden="true"></span><span class="sr-only">Previous</span></a><a class="right carousel-control" href="#carousel1" role="button" data-slide="next"><span class="glyphicon glyphicon-chevron-right" aria-hidden="true"></span><span class="sr-only">Next</span></a></div>
	  <article id="main">
	    <h1><img src="../images/projects_PetDesk.png" width="363" height="112" alt=""/></h1>
	    <h1>PetDesk Appointments API</h1>
          <p>In this project I will demo a service layer hit to a publicly exposed API at PetDesk
              to gather a set of Appointments. From this data subset we are concerned with only two datapoints:<br/>
            <li>Distribution of Appointment Types : Appointment types range from baths to dental work to surgery. Keep track of the services that are in the highest demand.</li>
            <li>Appointment Requests Received Per Month : Understand the distribution of your clientele. What months have been the busiest for the business?</li>
              <br/>From these two datapoints I will be gathering and processing frequency of load for 
              each specific item and will be storing <br/> them in the Database. 
              From there, I will provide two API endpoints:<br/><br/>
              <li>AppointmentTypeFrequency - to handle appointment types</li>
              <li>AppointmentRequestFrequency - to handle appointment requests</li>
              <br/>This page will allow you to create an account within the API
              which will allow you to authenticate with the API via Bearer <br/>
              token to request the output from the two respective endpoints.
              This will produce a report that is intended for view to <br/>
              the Veterinary Clinic for analysis of business load.<br/>
        
        </p>	    
	  </article>
            
        </table>
        <table width="200" border="1" cellspacing="10" cellpadding="10">
            <tbody>
            <tr>
                <td>Test Controls<asp:Button id="btnRunJsonService" onclick="runJsonService_Click" runat="server" Text="Test JSON Service Refresh"></asp:Button>Test the hit to the JSON service for a database refresh.</td>
                <td></td>
            </tr>
            </tbody>
        </table>
            <br/><br/>
        <asp:Panel ID="pnlCreateUser" runat="server" DefaultButton="btnNewRegister">
            <table style="width: 55%" class="ms-list2-main" border="1">
                <!-- fpstyle: 25,011111100 -->
                <tr>
                    <td class="auto-style3"><strong>New User Registration</strong></td>
                    <td class="auto-style14">&nbsp;</td>
                    <td class="auto-style9">
                        <asp:Button id="btnNewLogin" runat="server" Text="Go To Login" Width="150px" class="ms-list2-odd" OnClick="btnNewLogin_Click" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style12">Email 
                        :</td>
                    <td class="auto-style15">
                        <asp:TextBox id="txtEmail" runat="server" Width="250px"></asp:TextBox>
                    </td>
                    <td class="ms-list2-even" style="height: 45px"></td>
                </tr>
                <tr>
                    <td class="auto-style13">Password :</td>
                    <td class="auto-style6">
                        <asp:TextBox id="txtPassword" TextMode="Password" runat="server" Width="250px"></asp:TextBox>
                    </td>
                    <td class="auto-style5"><strong>Password rules: must contain 1 upper 
                        case letter, one lower case letter, 1 symbol, and one numeric.</strong></td>
                </tr>
                <tr>
                    <td class="auto-style13">Confirm Password :</td>
                    <td class="auto-style7">
                        <asp:TextBox id="txtPasswordConfirm" TextMode="Password" runat="server" Width="250px"></asp:TextBox>
                    </td>
                    <td class="auto-style4">
                        <asp:Label ID="lblNewUserRegistgrationStatusText" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style8">
                        &nbsp;</td>
                    <td class="auto-style16">&nbsp;</td>
                    <td class="ms-list2-odd">
                        <asp:Button id="btnNewRegister" runat="server" Text="Register"  Width="150px" OnClick="btnNewRegister_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
            
        <asp:Panel ID="pnlLogin" runat="server" DefaultButton="btnExistingLogin">
            <table style="width: 55%" class="ms-list2-main" border="1">
                <!-- fpstyle: 25,011111100 -->
                <tr>
                    <td class="auto-style21"><strong>Existing User Login</strong></td>
                    <td class="auto-style22"></td>
                    <td class="auto-style23">
                        <asp:Button id="btnExistingRegister" runat="server" Text="Go To Register" Width="150px" class="ms-list2-odd" OnClick="btnExistingRegister_Click" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style12">User Name :</td>
                    <td class="auto-style15">
                        <asp:TextBox id="txtExistingEmail" runat="server" Width="250px"></asp:TextBox>
                    </td>
                    <td class="ms-list2-even" style="height: 45px"></td>
                </tr>
                <tr>
                    <td class="auto-style13">Password :</td>
                    <td class="auto-style6">
                        <asp:TextBox id="txtExistingPassword" TextMode="Password" runat="server" Width="250px"></asp:TextBox>
                    </td>
                    <td class="auto-style5">
                        <asp:Label ID="lblExistingStatusText" runat="server" Text=""></asp:Label>&nbsp;</td>
                </tr>               
                <tr>
                    <td class="auto-style8">
                        &nbsp;</td>
                    <td class="auto-style16">&nbsp;</td>
                    <td class="ms-list2-odd">
                        <asp:Button id="btnExistingLogin" runat="server" Text="Login"  Width="150px" OnClick="btnExistingLogin_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        
        <asp:Panel ID="pnlApiRequest" runat="server" DefaultButton="btnApiRequest">
            <table style="width: 55%" class="ms-list2-main" border="1">
                <!-- fpstyle: 25,011111100 -->
                <tr>
                    <td class="auto-style17"><strong>Api Report Request</strong></td>
                    <td class="auto-style14">&nbsp;</td>
                    <td class="auto-style9">
                        <asp:Button id="btnLogout" runat="server" Text="Log Out" Width="150px" class="ms-list2-odd" OnClick="btnLogout_Click" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style18">AppointmentRequestFrequency Report : </td>
                    <td class="auto-style15">
                        <asp:Button id="btnApiRequest" runat="server" Text="Get Report"  Width="150px" OnClick="btnApiRequest_Click"  />
                        </td>
                    <td class="ms-list2-even" style="height: 45px"></td>
                </tr>
                <tr>
                    <td class="auto-style19">AppointmentTypeFrequency Report :</td>
                    <td class="auto-style6">
                        <asp:Button id="btnApiType" runat="server" Text="Get Report"  Width="150px" OnClick="btnApiType_Click"  />
                    </td>
                    <td class="auto-style5">
                        &nbsp;</td>
                </tr>               
                <tr>
                    <td class="auto-style20">
                        &nbsp;</td>
                    <td class="auto-style16">&nbsp;</td>
                    <td class="ms-list2-odd">
                        &nbsp;</td>
                </tr>
            </table>
        </asp:Panel>
        
        <asp:Panel ID="pnlGridView" runat="server">
            <table style="width: 55%" class="ms-list2-main" border="1">
            <tbody>
            <tr display="block" runat="server" >
                <td class="centerBtn" >
                    <asp:Label ID="lblGridViewHeader" runat="server" Text="" CssClass="auto-style17"></asp:Label>
                    <asp:GridView ID="dgvResults" runat="server" CssClass="myGridClass">
                    </asp:GridView>                   
                </td>
            </tr>
        </tbody>
      </table>
        </asp:Panel>

   	  <article id="main">
	    <h4>Project Links:<br/>
            <li><a href="http://khenzel.info:8700/Assets/Projects/PetDeskAPI" target="_blank">PetDesk Appointments API Endpoint</a></li>
            <li><a href="http://khenzel.info:8700/webForms/petDeskGetAppointments.aspx" target="_blank">PetDesk Get Appointments JSON Service</a></li>
            <li><a href="http://khenzel.info:8700/webForms/petDeskGetAppointments.aspx?debugmode=1" target="_blank">PetDesk Get Appointments JSON Service - debug mode enabled</a></li>
            <li><a href="https://github.com/khenzel/PetDeskAppointmentsAPI" target="_blank">GitHub Repository link to project - PetDeskAppointmentsAPI</a></li>
            <li><a href="../Assets/Portfolio/Projects_PetDesk_Appointments_WebApi-Design_Document_v1.pdf" target="_blank">Design Document: Appointments Web API Coding Exercise - PetDesk</a></li>
        &nbsp;</h4>
	    <p>&nbsp;</p>
	  </article>
      <article id="main">
          <p>If you have any questions, please feel free to<!-- #BeginLibraryItem "/library/lcContact.lbi" -->
              <a href="../contact.aspx">contact me</a> <!-- #EndLibraryItem -->.</p>
          <p><a href="../landing.aspx">Return to the Landing Page</a></p>
          <a href="#">Go to top</a>
	  </article>
	  <div class="row"></div>
	  <footer><!-- #BeginLibraryItem "/library/Footer.lbi" -->


<style type="text/css">
<!--

/*gridview styling css*/
    .myGridClass {
        width: 100%;
        /*this will be the color of the odd row*/
        background-color: #fff;
        margin: 5px 0 10px 0;
        border: solid 1px #525252;
        border-collapse:collapse;
    }

    /*data elements*/
    .myGridClass td {
        padding: 2px;
        border: solid 1px #c1c1c1;
        color: #717171;
    }

    /*header elements*/
    .myGridClass th {
        padding: 4px 2px;
        color: #fff;
        background: #424242;
        border-left: solid 1px #525252;
        font-size: 0.9em;
    }

    /*his will be the color of even row*/
    .myGridClass .myAltRowClass { background: #fcfcfc repeat-x top; }

    /*and finally, we style the pager on the bottom*/
    .myGridClass .myPagerClass { background: #424242; }

    .myGridClass .myPagerClass table { margin: 5px 0; }

    .myGridClass .myPagerClass td {
        border-width: 0;
        padding: 0 6px;
        border-left: solid 1px #666;
        font-weight: bold;
        color: #fff;
        line-height: 12px;
    }

    .myGridClass .myPagerClass a { color: #666; text-decoration: none; }

    .myGridClass .myPagerClass a:hover { color: #000; text-decoration: none; } 
/*end gridview styling css*/

.LibraryFooter {
	color: #000;
}
body {
	background-color: #FFF;
}
.LibraryFooter a u {
	color: #09C;
}
h5 {
	padding-top: 25px;
	padding-bottom: 25px;
	background-color: #9D9D9D;
	color: #F5F5F5;
}
#footer {
	color: #F5F5F5;
}

.ms-list2-main {
    border-left-style: none;
    border-right-style: none;
    border-top-style: none;
    border-bottom: 1.5pt solid gray;
}
.ms-list2-tl {
    font-weight: bold;
    color: white;
    border-left-style: none;
    border-right-style: none;
    border-top-style: none;
    border-bottom: .75pt solid black;
    background-color: #008078;
}
.ms-list2-left {
    border-style: none;
}
.ms-list2-top {
    font-weight: bold;
    color: white;
    border-left-style: none;
    border-right-style: none;
    border-top-style: none;
    border-bottom: .75pt solid black;
    background-color: #008078;
}
.ms-list2-even {
    font-weight: normal;
    color: black;
    border-style: none;
    background-color: white;
}
.ms-list2-odd {
    font-weight: normal;
    color: black;
    border-style: none;
    background-color: #EFFFEF;
}
.auto-style3 {
    font-weight: bold;
    color: white;
    border-left-style: none;
    border-right-style: none;
    border-top-style: none;
    border-bottom: .75pt solid black;
    background-color: #008078;
    text-align: center;
        width: 396px;
        font-family: Arial;
        font-size: medium;
    }
.auto-style4 {
    border-style: none;
    font-size: x-small;
}
.auto-style5 {
    font-weight: normal;
    color: black;
    border-style: none;
    background-color: white;
    font-size: x-small;
}
.auto-style6 {
    font-weight: normal;
    color: black;
    border-style: none;
    background-color: white;
    text-align: center;
        width: 445px;
    }
.auto-style7 {
    border-style: none;
    text-align: center;
        width: 445px;
    }
.auto-style8 {
    font-weight: normal;
    color: black;
    border-style: none;
    background-color: #EFFFEF;
    text-align: center;
        width: 396px;
    }
.auto-style9 {
    font-weight: bold;
    color: white;
    border-left-style: none;
    border-right-style: none;
    border-top-style: none;
    border-bottom: .75pt solid black;
    background-color: #008078;
    text-align: right;
}

    .auto-style12 {
        border-style: none;
        font-family: Arial;
        font-size: small;
        height: 45px;
        width: 396px;
    }
    .auto-style13 {
        border-style: none;
        font-family: Arial;
        font-size: small;
        width: 396px;
    }
    .auto-style14 {
        font-weight: bold;
        color: white;
        border-left-style: none;
        border-right-style: none;
        border-top-style: none;
        border-bottom: .75pt solid black;
        background-color: #008078;
        width: 445px;
    }
    .auto-style15 {
        font-weight: normal;
        color: black;
        border-style: none;
        background-color: white;
        text-align: center;
        height: 45px;
        width: 445px;
    }
    .auto-style16 {
        font-weight: normal;
        color: black;
        border-style: none;
        background-color: #EFFFEF;
        width: 445px;
    }

    .auto-style17 {
        font-weight: bold;
        color: white;
        border-left-style: none;
        border-right-style: none;
        border-top-style: none;
        border-bottom: .75pt solid black;
        background-color: #008078;
        text-align: center;
        width: 555px;
        font-family: Arial;
        font-size: medium;
    }
    .auto-style18 {
        border-style: none;
        font-family: Arial;
        font-size: small;
        height: 45px;
        width: 555px;
    }
    .auto-style19 {
        border-style: none;
        font-family: Arial;
        font-size: small;
        width: 555px;
    }
    .auto-style20 {
        font-weight: normal;
        color: black;
        border-style: none;
        background-color: #EFFFEF;
        text-align: center;
        width: 555px;
    }

    .auto-style21 {
        font-weight: bold;
        color: white;
        border-left-style: none;
        border-right-style: none;
        border-top-style: none;
        border-bottom: .75pt solid black;
        background-color: #008078;
        text-align: center;
        width: 396px;
        font-family: Arial;
        font-size: medium;
        height: 53px;
    }
    .auto-style22 {
        font-weight: bold;
        color: white;
        border-left-style: none;
        border-right-style: none;
        border-top-style: none;
        border-bottom: .75pt solid black;
        background-color: #008078;
        width: 445px;
        height: 53px;
    }
    .auto-style23 {
        font-weight: bold;
        color: white;
        border-left-style: none;
        border-right-style: none;
        border-top-style: none;
        border-bottom: .75pt solid black;
        background-color: #008078;
        text-align: right;
        height: 53px;
    }

    -->
</style>
<link href="../library/footer.css" rel="stylesheet" type="text/css">
<script type="text/javascript">
<!--
    function MM_openBrWindow(theURL,winName,features) { //v2.0
        window.open(theURL,winName,features);
    }
    //-->
</script>
<blockquote>
<span><p><hr>
  <h5 align="center" class="LibraryFooter" id="footer">Solutions Web &copy; Copyright 2015<br>
  <script language="Javascript"><!--
    document.write("Last updated: "+document.lastModified+"");

    --></script>
<br>
Site Developed & Maintained by <a href="mailto:kev.henzel@gmail.com?Subject=SolutionsWeb%20Contact%20Request&body=Please%20make%20your%20request%20below.%20I%20will%20get%20back%20to%20you%20as%20soon%20as%20possible!">Kevin Henzel</a><br>  
  <a href="../site_map.aspx">Site Map</a> | <a href="../contact.aspx">Contact Me</a><br >
  

			  
<br />


   </h5>
</blockquote>

<!-- #EndLibraryItem --></footer>
  </div>
	<!-- jQuery (necessary for Bootstrap's JavaScript plugins) --> 
	<script src="../js/jquery-1.11.2.min.js"></script>

	<!-- Include all compiled plugins (below), or include individual files as needed --> 
	<script src="../js/bootstrap.js"></script>
    </div>
    </form>
</body>
</html>
