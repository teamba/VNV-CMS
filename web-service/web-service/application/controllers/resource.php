<?php if ( ! defined('BASEPATH')) exit('No direct script access allowed');
date_default_timezone_set("PRC");

class resource extends CI_Controller {
	function customError($errno, $errstr)
 	{ 
 		echo "<b>Error:</b> [$errno] $errstr";
 	}

	public function quit()
	{
		$this->session->sess_destroy();
		//delete_cookie("cardId");
		echo  'logout .....';
	}

	public function test()
	{
		echo "resource API test!";
		//$this->load->view('welcome_message');
	}	

	function getparam($name)
	{
		$result = '';
		if (isset($_POST[$name])) $result = $_POST[$name];
		else if (isset($_GET[$name])) $result = $_GET[$name];

		return $result;
	}

	// get resource list in group: groupid, type
	function get_list()
	{
		header('Content-type: application/json');

		$groupid = $this->getparam("groupid");
		$type = $this->getparam("type");

		if ($groupid=='' || $type=='')
		{
			echo "";
			return;
		}

		$this->db->where('GroupID', $groupid);
		$this->db->where('ResourceType', $type);
		$this->db->select('ID, Title, Author, Source, Status');
		$this->db->order_by('ID', 'desc');
		$query = $this->db->get('t_Resource');

		$counter = 0;
		foreach ($query->result() as $row)
		{
			$data[$counter]['ID'] = $row->ID;
			$data[$counter]['Title'] = $row->Title;
			$data[$counter]['Author'] = $row->Author;
			$data[$counter]['Source'] = $row->Source;
			$data[$counter]['Status'] = $row->Status;

			$counter = $counter + 1;
		}

		if ($counter == 0) echo "";
		else echo json_encode($data);	
	}
}