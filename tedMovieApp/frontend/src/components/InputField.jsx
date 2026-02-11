import React from "react";

function InputField({name, label, type, value, onChange}){


    return(
        <div className = {name}>
            <label> 
                {label}
            </label>
            <input 
            name = {name}
            type = {type}
            value = {value}
            onChange = {onChange}/>
        </div>
    )

}


export default InputField;