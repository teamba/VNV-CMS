<?php if ( ! defined('BASEPATH')) exit('No direct script access allowed');
date_default_timezone_set("PRC");

class resource extends CI_Controller {
	function customError($errno, $errstr)
 	{ 
 		error_log("[$errno] $errstr",0);
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
		/*
		$data='{"Title":"my title","SubTitle":"sub title","GroupID":"1","Type":"1","Author":"1","Source":"","Brief":"","Status":"0","CreateDate":"2015-12-12","Content":"","UID":"","ParentUID":"","GroupUID":""}'; 

		$resource = json_decode($data);
		$resource->Content = "my content";

		$this->load->helper('date');
		$datestring = "%y-%m-%d %h:%i:%a";
		$time = time();
		$createdate = mdate($datestring, $time);
		$resource->CreateDate = $createdate;
		$resource->GroupID = 1;

		$this->db->insert('t_Resource', $resource);

		echo "insert resource success"; 	*/
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

		$ret['flag'] = 0;
		$ret['error'] = "";
		if ($groupid==null || $groupid=='')
		{
			$ret['flag'] = -1;
			$ret['error'] = "Please offer the group id";
			echo json_encode($ret);
			return;
		}

		$this->db->where('GroupID', $groupid);
		$this->db->select('ID, Title, CreateDate');
		$this->db->order_by('ID', 'desc');
		$query = $this->db->get('t_Resource');

		$counter = 0;
		foreach ($query->result() as $row)
		{
			$data[$counter]['ID'] = $row->ID;
			$data[$counter]['Title'] = $row->Title;
			$data[$counter]['CreateDate'] = $row->CreateDate;

			$counter = $counter + 1;
		}

		$ret['flag'] = $counter;
		$ret['items'] = $data;
		echo json_encode($ret);
	}

	function get_ex()
	{
		header('Content-type: application/json');
		$id = $this->getparam("id");

		$ret['flag'] = 0;
		$ret['error'] = "";
		if ($id==null || $id=="") 
		{
			$ret['flag'] = -1;
			$ret['error'] = "Please offer the resource id";
			echo json_encode($ret);
			return;
		}

		$this->db->limit(1);
		$this->db->where('ID', $id);
		$data = $this->db->get('t_Resource')->row();

		if ($data)
		{
			$ret['flag'] = 1;
			$ret['items'] = $data;
		}
		else 
		{
			$ret['flag'] = -1;
			$ret['error'] = "cann't find the resource.";
		}

		echo json_encode($ret);
	}

	function update() {
		header('Content-type: application/json');

		$strResourceEx = $this->getparam('strResourceEx');
		$content = $this->getparam('content');

		$ret['flag'] = 0;
		$ret['error'] = "";
		if ($strResourceEx==null || $strResourceEx=="")
		{
			$ret['flag'] = -1;
			$ret['error'] = "Please offer the resource contetn.";
			echo json_encode($ret);
			return;
		}

		$resource = json_decode($strResourceEx);
		//error_log("resource id:".$resource->ID."\r\n", 0);
		$this->db->where('ID', $resource->ID);
		$this->db->update('t_Resource', $resource);

		$d['Content']=$content;
		$this->db->where('ID', $resource->ID);
		$this->db->update('t_Resource', $d);

		$ret['flag'] = 1;
		echo json_encode($ret);
	}

	function add()
	{
		header('Content-type: application/json');
		$ret['flag'] = 0;
		$ret['error'] = "";

		$strResourceEx = $this->getparam('strResourceEx');
		$content = $this->getparam('content');
		$groupid = $this->getparam('groupID');

		if ($strResourceEx==null || $strResourceEx=="")
		{
			$ret['flag'] = -1;
			$ret['error'] = "Please offer the resource contetn.";
			echo json_encode($ret);
			return;
		}

		$resource = json_decode($strResourceEx);
		$resource->Content = $content;
		
		$this->load->helper('date');
		$datestring = "%y-%m-%d %h:%i:%a";
		$time = time();
		$createdate = mdate($datestring, $time);
		$resource->CreateDate = $createdate;
		$resource->GroupID = $groupid;

		$this->db->insert('t_Resource', $resource);
		$ret['flag'] = $this->db->insert_id(); 

		echo json_encode($ret);
	}

	function delete()
	{
		header('Content-type: application/json');
		$ret['flag'] = 0;
		$ret['error'] = "";

		$id = $this->getparam('id');
		$this->db->where("ID", $id);
		$this->db->delete("t_Resource");
		
		echo json_encode($ret);
	}
}