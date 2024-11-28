import axios from 'axios';

const API_BASE_URL = 'http://localhost:5157/gateway';

export const fetchData = async () => {
  try {
    const response = await axios.get(`${API_BASE_URL}/currency-exchange/rates`);
    return response.data;
  } catch (error) {
    console.error('Error fetching data:', error);
    throw error;
  }
};