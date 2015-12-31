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
		//header('Content-type: application/json');


		$this->load->helper('date');
			 
		$datestring = "%y-%m-%d %h:%i:%a";
		date_default_timezone_set('PRC');
		$time = time();
		$createdate = mdate($datestring, $time);
		echo "date:".$createdate;
		//echo json_encode($ret);				
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
		$strColumnEx = $this->getparam("strColumnEx");
		
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

		$this->load->helper('date');
		date_default_timezone_set('PRC');	 
		$datestring = "%y-%m-%d %h:%i:%a";
		$time = time();
		$createdate = mdate($datestring, $time);
		$column->CreateDate = $createdate;
		
		$this->db->insert('t_Column', $column);
		$ret['flag'] = $this->db->insert_id(); 
		$ret['items'] = $column;
		
		//error_log("finish column add.", 0); 
		echo json_encode($ret);			
	}

	function update() {
		header('Content-type: application/json');

		$strColumnEx = $this->getparam('strColumnEx');
		if ($strColumnEx == "") {
			$ret['flag'] = -1;
			$ret['error'] = "please offer column data.";
			echo json_encode($ret);
			return;
		}
		
		$this->db->where('ID', $strColumnEx->ID);
		$this->db->update('t_Column', $strColumnEx);
		
		$result['flag'] = 0;
		$result['error'] = "";	

		echo json_encode($result);	
	}

	function delete() {
		header('Content-type: application/json');

		$column_id = $this->getparam('columnID');
		
		$this->db->where('ID', $column_id);
		$this->db->delete('t_Column');
		
		$result['flag'] = 0;
		$result['error'] = "";		

		echo json_encode($result);		
	}
	
	function get_edit_update() {
		header('Content-type: application/json');

		$columnid = $this->getparam("columnid");
		
		$ret['flag'] = 0;
		$ret['error'] = "";
		if ($columnid == null || $columnid=="")
		{
			$ret['flag'] = -1;
			$ret['error'] = "Please offer the column id.";
			echo json_encode($ret);
			return;
		}	
		
		/* SQL sample
		select t_UpdateItem.ID, t_ColumnUpdate.ID as uID,t_Resource.ID as rID,t_UpdateItem.Title, t_Resource.Title as rTitle, t_UpdateItem.Brief, t_UpdateItem.ListPoint 
		From t_UpdateItem 
		Join t_ColumnUpdate On t_UpdateItem.UpdateID=t_ColumnUpdate.ID 
		Join t_Resource on t_UpdateItem.ResourceID=t_Resource.ID 
		Where t_ColumnUpdate.OwnerID=0 and t_ColumnUpdate.Status=0 and t_ColumnUpdate.ColumnID=8
		*/
		$strSQL = "select t_UpdateItem.ID, t_ColumnUpdate.ID as uID,t_Resource.ID as rID,t_UpdateItem.Title, t_Resource.Title as rTitle, t_UpdateItem.Brief, t_UpdateItem.ListPoint ";
		$strSQL = $strSQL."From t_UpdateItem ";
		$strSQL = $strSQL."Join t_ColumnUpdate On t_UpdateItem.UpdateID=t_ColumnUpdate.ID  ";
		$strSQL = $strSQL."Join t_Resource on t_UpdateItem.ResourceID=t_Resource.ID ";
		$strSQL = $strSQL."Where t_ColumnUpdate.OwnerID=0 and t_ColumnUpdate.Status=0 and t_ColumnUpdate.ColumnID=".$columnid;	
			  
		$query = $this->db->query($strSQL);
		
		$counter = 0;
		foreach ($query->result() as $row)
		{
		   $data[$counter]['ID'] = $row->ID;
		   $data[$counter]['UpdateID'] = $row->uID;
		   $data[$counter]['ResourceID'] = $row->rID;
		   $data[$counter]['Title'] = $row->Title;
		   if ($data[$counter]['Title']=="") $data[$counter]['Title'] = $row->rTitle;
		   $data[$counter]['Brief'] = $row->Brief;
		   $data[$counter]['ListPoint'] = $row->ListPoint;
		   
		   $counter += 1;
		}
		
		$ret['flag'] = $counter;
		$ret['items'] = $data;
		
		echo json_encode($ret);
	}
	
	function save_edit_update_run($updateid, $columnid, $strUpdateItem) {
		$ret['flag'] = 0;
		$ret['error'] = "";
		if ($strUpdateItem == null || $strUpdateItem=="")
		{
			$ret['flag'] = -1;
			$ret['error'] = "Please offer the Update Items.";

			return $ret;
		}	

		$this->load->library('session');
		$user_id = $this->session->userdata('user_id');
		if ($user_id==null || $user_id=="") $user_id = 0;
				
		$tu = null;
		$this->db->limit(1);
		if ($updateid > 0) {
			$this->db->where("UpdateID", $updateid);
		}
		else {
			$this->db->where("OwnerID", $user_id);
			$this->db->where("Status", 0);			
		}
		$tu = $this->db->get('t_ColumnUpdate')->row;
		
		$strSQL = "Update t_ColumnUpdate Set Status=2 Where Status=0 And OwnerID=".userID." And ColumnID=".$columnid;				
		$this->db->query($strSQL);
		
		$updateID_backup = 0;
		
		$this->load->helper('date');
		$datestring = "%y-%m-%d %h:%i:%a";
		$time = time();
		$createdate = mdate($datestring, $time);
					
		if ($tu == null) {
			$tu['OwnerID'] = $user_id;
			$tu['ColumnID'] = $column_id;
			
			$tu['CreateDate'] = $createdate;	
			
			$this->db->insert('t_ColumnUpdate', $tu);
			$tu['ID'] = $this->db->insert_id();	
		}
		else {
			if ($tu['Status'] == 1) {
				$tu['Status'] = 2;			
				$this->db->where('ID', $tu['ID']);
				$this->db->update('t_ColumnUpdate', $tu);
				
				$updateID_backup = $tu['ID'];
				$tu['Status'] = 0;
				$tu['CreateDate'] = $createdate;
				$this->db->insert('t_ColumnUpdate', $tu);
			}
		}
		
		$tu['Status'] = 0;
		$this->db->where('ID', $tu['ID']);
		$this->db->update('t_ColumnUpdate', $tu);
		
		// get the exist update items
		$this->db->where('UpdateID', $updateID_backup);
		$query=$this->db->get('t_UpdateItem');
		$existNum = 0;
		foreach ($query->result() as $row)
		{
			$exist_items[$existNum]['ResourceID'] = $row['ResourceID'];
			$existNum = $existNum + 1;
		}
		
		// the new items
		// http://www.cnblogs.com/freespider/archive/2010/08/19/1803308.html
		// http://www.5idev.com/p-php_explode_str_split.shtml
		$new_items = explode('|', $strUpdateItem);
		
		for($i=0;i<count($new_items); $i++)
		{
			$be_new = 1;
			for($j=0;$j<$existNum; $j++)
			{
				if ($new_items[$i] == $exist_items[$j]['ResourceID'])
				{
					$be_new = 0;
					break;
				}
			}
			
			if ($be_new == 0) continue;
			
			$new_item['ResourceID'] = $new_items[$i];
			$new_item['ListPoint'] = $i+1;
			$new_item['UpdateID'] = $tu['ID'];
			$new_item['Title'] = "";
			$new_item['Brief'] = "";
			
			$this->db->insert('t_UpdateItem', $new_item);
			
			$exist_items[$existNum] = $new_items[$i];
			$existNum = $existNum + 1;
		}
		
		$ret['flag'] = $existNum;
		$ret['items'] = $tu; 		
	
		return $ret;
	}
	
	function save_edit_update() {
		header('Content-type: application/json');
		
		$columnid = $this->getparam("columnid");
		$updateid = $this->getparam("updateid");
		$strUpdateItem = $this->getparam("strUpdateItem");
		
		if ($columnid=="") $columnid=0;
		if ($updateid=="") $updateid=0;

		$ret = save_edit_update($updateid, $columnid, $strUpdateItem);
		
		echo json_encode($ret);
	}
	
	function save_all_update() {
		header('Content-type: application/json');
		
		$strUpdates = $this->getparam('strUpdates');
		
		$items1 = explode('-', $strUpdates);
		for($i=0;$i<count($items1);$i++) {
			$items2 = explode(':', $items1[$i]);
			if (count($items2) != 2) continue;
			
			save_edit_update_run(0, $items2[0], $items2[1]);
		}
		
		$ret['flag'] = count($items1);
		$ret['error'] = "";
		
		echo json_encode($ret);
	}
	
	function save_update_item() {
		header('Content-type: application/json');
		
		$strUpdateItem = $this->getparam('strUpdateItem');
		if ($strUpdateItem == "") {
			$ret['flag'] = -1;
			$ret['error'] = "please offer the update item data.";
			echo json_encode($ret);
			return;
		}
		
		$item = json_decode($strUpdateItem);
		$this->db->where('ID', $item->ID);
		$this->db->update('t_UpdateItem', $item);
		
		$ret['flag'] = 0;
		$ret['error'] = "";
		
		echo json_encode($ret);
	}
	
	function get_updates() {
		header('Content-type: application/json');
		
		$column_id = $this->getparam('columnID');
		$this->load->library('session');
		$user_id = $this->session->userdata('user_id');
		if ($user_id==null || $user_id=="") $user_id = 0;
		
		$this->db->where('ColumnID', $column_id);
		$this->db->where('OwnerID', $user_id);
		$this->db->oerder_by('CreateDate', 'desc');
		$query =  $this->db->get('t_ColumnUpdate');
		
		$counter = 0;
		foreach ($query->result() as $row) {
			$updates[$counter]['ID'] = $row->ID;
			$updates[$counter]['OwnerID'] = $row->OwnerID;
			$updates[$counter]['CreateDate'] = $row->CreateDate;
			$updates[$counter]['Status'] = $row->Status;
			
			$counter = $counter + 1;
		}
				
		$ret['flag'] = $counter;
		$ret['error'] = "";
		$ret['items'] = $updates;
		
		echo json_encode($ret);		
	}
	
	function get_update_items() {
		header('Content-type: application/json');
		
		$update_id = $this->getparam('updateID');
		$this->load->library('session');
		$user_id = $this->session->userdata('user_id');
		if ($user_id==null || $user_id=="") $user_id = 0;
		/*
		select t_UpdateItem.ID, t_Resource.ID as rID, t_UpdateItem.Title, t_Resource.Title as rTitle, t_UpdateItem.Brief,ListPoint 
		From t_UpdateItem 
		Join t_Resource on t_UpdateItem.ResourceID=t_Resource.ID 
		Where UpdateID=1		
		*/
		$strSQL = "select t_UpdateItem.ID, t_Resource.ID as rID, t_UpdateItem.Title, t_Resource.Title as rTitle, t_UpdateItem.Brief,ListPoint ";
		$strSQL = $strSQL."From t_UpdateItem ";
		$strSQL = $strSQL."Join t_Resource on t_UpdateItem.ResourceID=t_Resource.ID ";
		$strSQL = $strSQL."Where UpdateID=".$update_id;

		$query = $this->db->query($strSQL);
		
		$counter = 0;
		foreach ($query->result() as $row)
		{
		   $data[$counter]['ID'] = $row->ID;
		   $data[$counter]['ResourceID'] = $row->rID;
		   $data[$counter]['Title'] = $row->Title;
		   if ($data[$counter]['Title']=="") $data[$counter]['Title'] = $row->rTitle;
		   $data[$counter]['Brief'] = $row->Brief;
		   $data[$counter]['ListPoint'] = $row->ListPoint;
		   
		   $counter += 1;
		}
				
		$ret['flag'] = $counter;
		$ret['error'] = "";
		$ret['items'] = $data;
		
		echo json_encode($ret);					
	}
	
	function delete_update_item() {
		header('Content-type: application/json');
		
		$update_id = $this->getparam('UpdateID');
		$resource_id = $this->getparam('ResourceID');
		
		$strSQL = "Delete From t_UpdateItem Where UpdateID=".$update_id." And ResourceID=".$resource_id;
		$this->db->query($strSQL);
		
		$ret['flag'] = 0;
		$ret['error'] = "";
		
		echo json_encode($ret);			
	}
}