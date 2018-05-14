
function CreateFeature() {
        $.ajax({
            type: 'Post',
            dataType: 'Json',
            data:
            {
                txtFeature: $('#txtFeature').val()
            },
            url: '/Property/PropertyFeature/CreateFeature',
            success: function (da) {
                if (da.Result == "Success") {

                    document.location.reload(true)
                }
                else {

                    alert('Error' + da.Message);
                }
            },
            error: function (da) {
                debugger;
                alert('Error');
            }
        });
        
      
    }


function CreatePriceType() {
    debugger;
    $.ajax({
        type: 'Post',
        dataType: 'Json',
        data:
        {
            txtTypeName: $('#txtTypeName').val()
        },
        url: '/Property/PropertyPrice/CreateType',
        success: function (da) {
            if (da.Result == "Success") {

                document.location.reload(true)
            }
            else {

                alert('Error' + da.Message);
            }
        },
        error: function (da) {
       
            alert('Error');
        }
    });


}



