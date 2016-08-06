$(document).ready(function(){

	$("button").addClass("btn btn-xs btn-default");

	//when user clicks add button
	$("#add").click(function(){
		var query = $("#courseInput").val();

		$("#userList").append(
			$("<li>").append(query).append(
					$("<button>").attr('class', 'btn btn-xs btn-default deleteItem').append("delete")
			)
		);
		$("#courseInput").val("");	
	});

	//when user clicks delete button
	$(document).on('click', '.deleteItem', (function() {
		$(this).parent().remove();
	}));
});