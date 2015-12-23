<?php if ( ! defined('BASEPATH')) exit('No direct script access allowed');

class user extends CI_Controller {
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
		echo "user API test!";
		//$this->load->view('welcome_message');
	}	

	function getparam($name)
	{
		$result = '';
		if (isset($_POST[$name])) $result = $_POST[$name];
		else if (isset($_GET[$name])) $result = $_GET[$name];

		return $result;
	}

	// input: account, password
	function login()
	{
		header('Content-type: application/json');

		$account = $this->getparam('account');
		$password = $this->getparam('password');

		if ($account == '')
		{
			$ret['ok'] = 0;
			$ret['msg'] = 'please input the account!';
			echo json_encode($ret);		
			return;
		}

		$this->db->limit(1);
		$this->db->select('ID, Password, Name');
		$this->db->where('Account', $account);
		$data = $this->db->get('t_User')->row();

		if ($data)
		{
			if($data->Password == $password)
			{
				$ret['ok'] = 1;
				$ret['ID'] = $data->ID;
				$ret['Name'] = $data->Name;
			}
			else
			{
				$ret['ok'] = 0;
				$ret['msg'] = "account or password isn't valid!";
			}
		}
		else
		{
			$ret['ok'] = 0;
			$ret['msg'] = 'User account not exist!';
		}

		echo json_encode($ret);		
	}
}
