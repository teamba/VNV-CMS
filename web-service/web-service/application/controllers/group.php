<?php if ( ! defined('BASEPATH')) exit('No direct script access allowed');
date_default_timezone_set("PRC");

class group extends CI_Controller {
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
		echo "group API test!";
		//error_log("group API test!\r\n",0);
		//$this->load->view('welcome_message');

		echo "session is ".$this->session->userdata('group_id');
		//"Code":"myCode",
	/*	$data='{"ParentID":"1","Name":"my Group","Code":"myCode","Brief":"the Brief","Type":"1","CreateDate":"2015-12-12 10:10:10","UID":"","ParentUID":""}'; 

		$group = json_decode($data);
		$group->ParentID = 1;
		echo $group->Name; 
		$this->load->helper('date');
			 
		$datestring = "%y-%m-%d %h:%i:%a";
		$time = time();
		$createdate = mdate($datestring, $time);
		//$group->CreateDate =  "2015-12-12"; // $createdate;
		
		$this->db->insert('t_Group', $group);
		$id = $this->db->insert_id(); 

		echo "insert success, id:".$id; */
	}	

	function getparam($name)
	{
		$result = '';
		if (isset($_POST[$name])) $result = $_POST[$name];
		else if (isset($_GET[$name])) $result = $_GET[$name];

		return $result;
	}

	// search groups: parentid, type
	function search()
	{
		header('Content-type: application/json');

		$parentid = $this->getparam('parentid');
		$type = $this->getparam('type');

		if ($parentid == '' || $type == '')
		{
			$ret['ok'] = 0;
			$ret['msg'] = 'please input the parent id and type!';
			echo json_encode($ret);		
			return;
		}

		$ret['ok'] = 1;
		$ret['groups'] = $this->get_groups($parentid, $type);

		echo json_encode($ret);		
	}

	function get_groups($parentid, $type)
	{
		$groups = null;

		$this->db->where('ParentID', $parentid);
		$this->db->where('Type', $type);
		$query = $this->db->get('t_Group');

		$counter = 0;
		foreach ($query->result() as $row)
		{
			$groups[$counter]['ID'] = $row->ID;
			$groups[$counter]['Name'] = $row->Name;
			$groups[$counter]['Brief'] = $row->Brief;
			$groups[$counter]['ParentID'] = $parentid;
			
			$counter = $counter + 1;
		}

		for($i=0; $i<$counter; $i++)
		{
			$groups[$i]['groups'] = $this->get_groups($groups[$i]['ID'], $type);
		}

		return $groups;
	}
	
	/* new version code, added in 2015.12.22 */
	function get_all()
	{
		header('Content-type: application/json');
		//echo "enter get_all";
		$type = $this->getparam('Type');
		
		$result['flag'] = 0;
		$result['error'] = "";
		
		if ($type==null) 
		{
			$result['flag']=-1;
			$result['error'] = "Please input the grout Type.";
			echo json_encode($result);
			return;
		}
		
		$groups = null;

		$this->db->where('Type', $type);
		$query = $this->db->get('t_Group');

		$counter = 0;
		foreach ($query->result() as $row)
		{
			$groups[$counter]['ID'] = $row->ID;
			$groups[$counter]['Name'] = $row->Name;
			$groups[$counter]['Brief'] = $row->Brief;
			$groups[$counter]['ParentID'] = $row->ParentID;
			
			$counter = $counter + 1;
		}
		
		$result['flag'] = $counter;
		$result['items'] = $groups;

		echo json_encode($result);
	}

	// get one group info: input -- id
	function get() 
	{
		header('Content-type: application/json');
		//$this->load->library('session');

		$id = $this->getparam("id");
		$ret['flag'] = 0;
		$ret['error'] = "";

		if ($id==null || $id == "")
		{
			$ret['ok'] = -1;
			$ret['error'] = "Please input the group id.";
		}
		else
		{
			$this->db->limit(1);
			$this->db->where('ID', $id);
			$data = $this->db->get('t_Group')->row();

			if($data)
			{
				$ret['flag'] = 1;

				$group['ID'] = $data->ID;
				$group['Name'] = $data->Name;
				$group['Brief'] = $data->Brief;
				$group['ParentID'] = $data->ParentID;
				$group['Type'] = $data->Type;
				$group['Code']= $data->Code;

				$ret['items'] = $data;// $group;
				//$this->session->set_userdata('group_id', $data->ID);
			}
			else
			{
				$ret['flag'] = -1;
				$ret['error'] = "cann't found the group. id=".$id;
				//$this->session->set_userdata('group_id', 0);
			}
		}
		echo json_encode($ret);	
	}

	// add a new group: parentid, name, brief, type
	// reference http://www.3lian.com/edu/2014/02-11/128395.html
	function add()
	{
		header('Content-type: application/json');

		// sample code
		/*
		$data='[{"Name":"a1","Number":"123","Contno":"000","QQNo":""},{"Name":"a1","Number":"123","Contno":"000","QQNo":""},{"Name":"a1","Number":"123","Contno":"000","QQNo":""}]'; 
		$x = json_decode($data);
		echo  "Name:".$x[0]->Name." Number:".$x[0]->Number; */

		$parentid = $this->getparam("parentid");
		$strGroupEx = $this->getparam("strGroupEx");
		
		$ret['flag'] = 0;
		$ret['error'] = "";
		if ($parentid == null || $strGroupEx==null)
		{
			$ret['flag'] = -1;
			$ret['error'] = "Please input the parent id and group info";
			echo json_encode($ret);
			return;
		}
		//error_log("enter add:".$strGroupEx."\r\n", 0);
		$group = json_decode($strGroupEx);
		 
		if ($parentid == '') $parentid = "0";
		$group->ParentID = $parentid;

		$this->load->helper('date');
			 
		//$datestring = "%y-%m-%d %h:%i:%a";
		//$time = time();
		//$createdate = mdate($datestring, $time);

		//$data['Code'] = $group->Code;  // Attention: !!!!!
		
		$this->db->insert('t_Group', $group);
		$ret['flag'] = $this->db->insert_id(); 
		 
		echo json_encode($ret);			
	}

	// update a group: input -- group object
	function update()
	{
		header('Content-type: application/json');
		//error_log("enter update\r\n",0);
		
		$strGroupEx = $this->getparam('strGroupEx');
		
		if ($strGroupEx==null|| $strGroupEx=="")
		{
			$ret['ok'] = 0;
			$ret['error'] = "Please the group content!"; 
		}
		else
		{
			//error_log("group json string:".$strGroupEx."\r\n",0);
			$group = json_decode($strGroupEx);
			//error_log("group name: ".$group->Name."\r\n", 0);
			$this->db->where('ID', $group->ID);
			$this->db->update('t_Group', $group);

			$ret['flag'] = 1;
			$ret['error'] = "";
		}

		echo json_encode($ret);	
	}

	// delete a group: groupID
	function delete()
	{
		header('Content-type: application/json');

		$ret['flag'] = -1;
		$ret['error'] = "";
		$id = $this->getparam('groupID');
		if ($id==null || $id == '')
		{
			$ret['error'] = "Please input the group id.";
		}
		else
		{
			$this->db->where("ParentID", $id);
			$this->db->from("t_Group");
			$sum = $this->db->count_all_results();

			// not child group
			if ($sum > 0) {$ret['error'] = "Sub group exist.";}
			else
			{
				$this->db->where("GroupID", $id);
				$this->db->from("t_Resource");
				$sum = $this->db->count_all_results();
				// not resource
				if ($sum>0) $ret['error'] = "Resource exist in this group.";
				else
				{
					$this->db->where("ID", $id);
					$this->db->delete("t_Group");

					$ret['flag'] = 0;
				}
			}
		}
		
		echo json_encode($ret);
	}
}
