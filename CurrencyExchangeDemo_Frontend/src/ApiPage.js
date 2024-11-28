import React, { useEffect, useState, useRef } from 'react';
import { fetchData } from './api';

const ApiPage = () => {
  const [data, setData] = useState({ rates: [] });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [previousRates, setPreviousRates] = useState([]); // Store previous rates  calculate changes

  const intervalRef = useRef(null);

  
  const calculateRateChange = (oldRate, newRate) => {
    if (!oldRate || oldRate === 0) {
      console.log(`Rate change calculation skipped for oldRate: ${oldRate}, newRate: ${newRate}`);
      return 0; 
    }
    const rateChange = ((newRate - oldRate) / oldRate) * 100;
    console.log(`Rate change calculated: ${oldRate} -> ${newRate}, Change: ${rateChange}%`);
    return rateChange;
  };


  
  const getData = async () => {
    try {
      const result = await fetchData(); 
      console.log('Full API response:', result);
      
      if (result && result.value) {
        const newRates = result.value;
        console.log('New Rates:', newRates);

        if (previousRates.length > 0) {
          const updatedRates = newRates.map((item, index) => {
            const previousRate = previousRates[index]?.rate || 0;
            const rateChange = calculateRateChange(previousRate, item.rate);
            return { ...item, rateChangePercent: rateChange };
          });
          console.log('Updated Rates with Rate Change:', updatedRates);
          setData({ rates: updatedRates }); 
        } else {
          setData({ rates: newRates });
          console.log('First fetch, no rate change calculation');
        }

        setPreviousRates(newRates);
      } else {
        setError('Invalid API response format');
      }
    } catch (err) {
      console.error('Error in ApiPage:', err.message);
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    getData();

    intervalRef.current = setInterval(() => {
      getData(); 
    }, 5000); 
    
    return () => clearInterval(intervalRef.current);
  }, []); 

  useEffect(() => {
    if (!loading && previousRates.length === 0) {
      
      getData();
    }
  }, [loading, previousRates]); 

  if (loading) return <p>Loading...</p>;
  if (error) return <p>Error: {error}</p>;


  
  const rates = data.rates;

  const renderRateChangeArrow = (rateChange) => {
    if (rateChange === 'N/A') return <span>N/A</span>;
    return (
      <span
        style={{
          color: rateChange > 0 ? 'green' : rateChange < 0 ? 'red' : 'black',
        }}
      >
        {rateChange > 0 ? '↑' : rateChange < 0 ? '↓' : '→'} {/* ↑ for up, ↓ for down */}
      </span>
    );
  };

  return (
    <div>
      <h1>API Data</h1>
      {rates.length > 0 ? (
        <table border="1" style={{ width: '100%', borderCollapse: 'collapse' }}>
          <thead>
            <tr>
              <th>Base</th>
              <th>Target</th>
              <th>Rate</th>
              <th>Rate Change (%)</th>
              <th>Rate Change Arrow</th>
            </tr>
          </thead>
          <tbody>
            {rates.map((item, index) => (
              <tr key={index}>
                <td>{item.base}</td>
                <td>{item.target}</td>
                <td>{item.rate}</td>
                <td>
                  {item.rateChangePercent !== undefined
                    ? item.rateChangePercent.toFixed(2)
                    : 'N/A'}
                </td>
                <td>
                  {renderRateChangeArrow(item.rateChangePercent)} {/* Arrow and color */}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      ) : (
        <p>No data available</p>
      )}
    </div>
  );
};

export default ApiPage;