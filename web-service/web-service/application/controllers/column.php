<?php if ( ! defined('BASEPATH')) exit('No direct script access allowed');

class column extends CI_Controller {
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
		echo "column API test!";
		//$this->load->view('welcome_message');
		header('Content-type: application/json');

		$strColumnEx = '{ "Name": "new column", "Code": "new-column", "Brief":"", "CreateDate":"2015-12-12" }';
		$column = json_decode($strColumnEx);
		 
		$column->ParentID = 1;
/*
		$this->load->helper('date');
			 
		$datestring = "%y-%m-%d %h:%i:%a";
		$time = time();
		$createdate = mdate($datestring, $time);
		$column->CreateDate = $createdate;
*/		
		$this->db->insert('t_Column', $column);
		$ret['flag'] = $this->db->insert_id(); 

		echo json_encode($ret);				
	}	

	function getparam($name)
	{
		$result = '';
		if (isset($_POST[$name])) $result = $_POST[$name];
		else if (isset($_GET[$name])) $result = $_GET[$name];

		return $result;
	}

	// get the update id according to the column code
	function get_updateID()
	{
		$code = $this->getparam('code');
		if ($code == '') 
		{
			return 0;
		}

		$this->db->limit(1);
		$this->db->where('Code', $code);
		//$this->db->select('ID');
		$this->db->order_by('ID', 'desc');
		$data = $this->db->get('t_Column')->row();
		if ($data)
		{
			$column_id = $data->ID;
		}
		else
		{
			return 0;			
		}

		$this->db->limit(1);
		$this->db->where('ColumnID', $column_id);
		$this->db->order_by('ID', 'desc');
		$this->db->select('ID');
		$data = $this->db->get('t_ColumnUpdate')->row();
		if ($data)
		{
			$update_id = $data->ID;
		}
		else
		{
			return 0;			
		}

		return $update_id;
	}

	// input: column code
	function get_images()
	{
		header('Content-type: application/json');

		$code = $this->getparam('code');
		if ($code == '') 
		{
			$ret['ok']=0;
			$ret['msg']='Need the column code';
			echo json_encode($ret);
			return;
		}

		$this->db->limit(1);
		$this->db->where('Code', $code);
		//$this->db->select('ID');
		$this->db->order_by('ID', 'desc');
		$data = $this->db->get('t_Column')->row();
		if ($data)
		{
			$column_id = $data->ID;
		}
		else
		{
			$ret['ok'] = 0;
			$ret['msg'] = 'column code not exist';
			echo json_encode($ret);
			return;			
		}

		$this->db->limit(1);
		$this->db->where('ColumnID', $column_id);
		$this->db->order_by('ID', 'desc');
		$this->db->select('ID');
		$data = $this->db->get('t_ColumnUpdate')->row();
		if ($data)
		{
			$update_id = $data->ID;
		}
		else
		{
			$ret['ok'] = 0;
			$ret['column ID'] = $column_id;
			$ret['msg'] = 'no release items in this column';
			echo json_encode($ret);
			return;			
		}

		$this->db->where('UpdateID', $update_id);
		$this->db->order_by('ListPoint', 'desc');
		$this->db->select('ResourceID');
		$query = $this->db->get('t_UpdateItem');

		$counter = 0;
		foreach ($query->result() as $row)
		{
			$resource_ids[$counter] = $row->ResourceID;
			$counter = $counter + 1;
		}

		$counter = 0;
		$this->db->where_in('ID', $resource_ids);
		$this->db->where('ResourceType', 2);
		$this->db->select('Title, Content');
		$query = $this->db->get('t_Resource');
		foreach ($query->result() as $row)
		{
			$images[$counter]['Title'] = $row->Title;
			$images[$counter]['URL'] = $row->Content;
			$counter = $counter + 1;
		}

		$ret['num'] = $counter;
		$ret['ok'] = 1;
		if ($counter>0) $ret['images'] = $images;

		echo json_encode($ret);
	}

	function get_article()
	{
		header('Content-type: application/json');

		$update_id = $this->get_updateID();
		if ($update_id == 0)
		{
			$ret['ok'] = 0;
			$ret['msg'] = 'no update in this column';
			echo json_encode($ret);			
			return;
		}

		$this->db->limit(1);
		$this->db->where('UpdateID', $update_id);
		$this->db->order_by('ListPoint', 'desc');
		$this->db->select('ResourceID');
		$data = $this->db->get('t_UpdateItem')->row();

		if ($data)
		{
			$resourceID = $data->ResourceID;
		}
		else
		{
			$ret['ok'] = 0;
			$ret['msg'] = 'no article';
			echo json_encode($ret);	
			return;			
		}
		
		$this->db->limit(1);
		$this->db->where('ID', $resourceID);
		$data = $this->db->get('t_Resource')->row();
		if ($data)
		{
			$ret['ok'] = 1;
			$ret['Title'] = $data->Title;
			$ret['Content'] = $data->Content;
		}
		else
		{
			$ret['ok'] = 0;
			$ret['msg'] = 'article missing';
		}

		echo json_encode($ret);	
	}

	function get_all()
	{
		header('Content-type: application/json');

		$result['flag'] = 0;
		$result['error'] = "";		

		$query = $this->db->get('t_Column');
		$counter = 0;
		foreach ($query->result() as $row)
		{
			$columns[$counter]['ID'] = $row->ID;
			$columns[$counter]['Name'] = $row->Name;
			$columns[$counter]['ParentID'] = $row->ParentID;
			
			$counter = $counter + 1;
		}

		$result['flag'] = $counter;	
		$result['items'] = $columns;

		echo json_encode($result);		
	}

	function get() {
		header('Content-type: application/json');

		$id = $this->getparam('id');

		$ret['flag'] = 0;
		$ret['error'] = "";		

		if ($id==null || $id == "")
		{
			$ret['flag'] = -1;
			$ret['error'] = "Please input the column id.";
		}
		else
		{
			$this->db->limit(1);
			$this->db->where('ID', $id);
			$data = $this->db->get('t_Column')->row();

			if ($data)
			{
				$ret['flag'] = 1;
				$ret['items'] = $data;
			}
			else
			{
				$ret['flag'] = -1;
				$ret['error'] = "cann't found the column.";
			}
		}
		echo json_encode($ret);	
	}

	function add() {
		//error_log("enter column add", 0);
		header('Content-type: application/json');

		$parentid = $this->getparam("parentID");
		$strGroupEx = $this->getparam("strGroupEx");
		
		$ret['flag'] = 0;
		$ret['error'] = "";
		if ($parentid == null || $strColumnEx==null)
		{
			$ret['flag'] = -1;
			$ret['error'] = "Please input the parent id and column info";
			echo json_encode($ret);
			return;
		}

		$column = json_decode($strColumnEx);
		 
		if ($parentid == '') $parentid = "0";
		$column->ParentID = $parentid;
/*
		$this->load->helper('date');
			 
		$datestring = "%y-%m-%d %h:%i:%a";
		$time = time();
		$createdate = mdate($datestring, $time);
		$column->CreateDate = $createdate;
	*/	
		$this->db->insert('t_Column', $column);
		$ret['flag'] = $this->db->insert_id(); 
		//error_log("finish column add.", 0); 
		echo json_encode($ret);			
	}

	function update() {
		header('Content-type: application/json');

		$result['flag'] = 0;
		$result['error'] = "";	

		echo json_encode($result);	
	}

	function delete() {
		header('Content-type: application/json');

		$result['flag'] = 0;
		$result['error'] = "";		

		echo json_encode($result);		
	}
}