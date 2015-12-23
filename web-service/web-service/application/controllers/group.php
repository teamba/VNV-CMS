<?php if ( ! defined('BASEPATH')) exit('No direct script access allowed');
date_default_timezone_set("PRC");

class group extends CI_Controller {
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
		echo "group API test!";
		//$this->load->view('welcome_message');
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

	// modify a group: input -- id, name, brief
	function modify()
	{
		header('Content-type: application/json');

		$id = $this->getparam('id');
		$name = $this->getparam('name');
		$brief = $this->getparam('brief');

		if ($id=="" || $name=="")
		{
			$ret['ok'] = 0;
			$ret['msg'] = "Please input the ID and Name!"; 
		}
		else
		{
			$data = array(
				'Name' => $name,
				'Brief' => $brief
				);

			$this->db->where('ID', $id);
			$this->db->update('t_Group', $data);

			$ret['ok'] = 1;
		}

		echo json_encode($ret);	
	}

	// delete a group: id
	function delete()
	{
		header('Content-type: application/json');

		$ret['ok'] = 0;
		$id = $this->getparam('id');
		if ($id == '')
		{
			$ret['msg'] = "Please input the group id.";
		}
		else
		{
			$this->db->where("ParentID", $id);
			$this->db->from("t_Group");
			$sum = $this->db->count_all_results();

			// not child group
			if ($sum > 0) {$ret['msg'] = "Sub group exist.";}
			else
			{
				$this->db->where("GroupID", $id);
				$this->db->from("t_Resource");
				$sum = $this->db->count_all_results();
				// not resource
				if ($sum>0) $ret['msg'] = "Resource exist in this group.";
				else
				{
					$this->db->where("ID", $id);
					$this->db->delete("t_Group");

					$ret['ok'] = 1;
				}
			}
		}
		
		echo json_encode($ret);
	}
	
	/* new version code, added in 2015.12.22 */
	function get_all()
	{
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

		$id = $this->getparam("id");
		$ret['flag'] = 0;
		$ret['error'] = "";

		if ($id==null || $id == "")
		{
			$ret['ok'] = -1;
			$ret['msg'] = "Please input the group id.";
		}
		else
		{
			$this->db->limit(1);
			$this->db->where('ID', $id);
			$data = $this->db->get('t_Group')->row();

			if ($data)
			{
				$ret['flag'] = 1;

				$group['ID'] = $data->ID;
				$group['Name'] = $data->Name;
				$group['Brief'] = $data->Brief;
				$group['ParentID'] = $data->ParentID;
				$group['Type'] = $data->Type;

				$ret['items'] = $group;
			}
			else
			{
				$ret['flag'] = 0;
				$ret['error'] = "cann't found the group.";
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
		
		$ret['flash'] = 0;
		$ret['error'] = "";
		if ($parentid == null || $strGroupEx==null)
		{
			$ret['flash'] = -1;
			$ret['error'] = "Please input the parent id and group info";
			echo json_encode($ret);
			return;
		}
		
		$group = json_decode($strGroupEx);

		if ($parentid == '') $parentid = "0";

		$this->load->helper('date');

		$datestring = "%y-%m-%d %h:%i:%a";
		$time = time();
		$createdate = mdate($datestring, $time);
		
		$data['ParentID'] = $parentid;
		$data['Name'] = $group->Name;
		$data['Brief'] = $group->Brief;
		$data['CreateDate'] = $createdate;
		$data['Type'] = $group->Type;
		$data['Code'] = $group->Code
		$this->db->insert('t_Group', $data);

		$ret['flag'] = $this->db->insert_id();

		echo json_encode($ret);	
		
	}
}
