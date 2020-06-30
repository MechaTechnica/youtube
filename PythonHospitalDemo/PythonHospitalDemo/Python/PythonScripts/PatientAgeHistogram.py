
# make sure that you have installed matplotlib with pip or conda
import matplotlib.pyplot as plt
from HospitalApi import patients

patient_ages = [patient.age for patient in patients]

n, bins, patches = plt.hist(patient_ages, 20, facecolor='green')

plt.xlabel('Age')
plt.ylabel('Frequency')
plt.title('Histogram of Snake ages')

plt.show()