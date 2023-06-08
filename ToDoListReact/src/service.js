import axios from 'axios';

axios.defaults.baseURL = "https://localhost:7255";

axios.interceptors.response.use(response=>{
  return response;
},error=>{
  console.log(Promise.reject(error));
})

export default {
  
  getTasks: async () => {
    const result = await axios.get(`/item`)    
    return result.data;
  },

  addTask: async(name)=>{
    console.log('addTask', name)
   const result = await axios.post(`/item`,{
      "id": 0,
      "name":name,
      "isComplete": false,
      "user_id": 0
    }, {
      headers: {
        'Content-Type': 'application/json'
      }
    })
    return result.data;
  },  
   
  setCompleted: async(id, isComplete)=>{
    console.log('setCompleted', {id, isComplete})
   const result = await axios.put(`/item/${id}`,{
      "id": 0,
      "name": "string",
      "isComplete": isComplete,
      "user_id": 0
    }, {
      headers: {
        'Content-Type': 'application/json'
      }
    })
     return result.data;
  },

  deleteTask:async(id)=>{
    const result = await axios.delete(`/item/${id}`)
    return result.data;
  }
};
