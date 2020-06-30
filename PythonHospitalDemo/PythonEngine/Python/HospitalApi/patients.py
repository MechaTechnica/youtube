import builtins

from HospitalApi.patient import Patient


class Patients:
	def __init__(self):
		self.patient_service = builtins.patient_service
		self.patients = self.patient_service.Patients()

	def __len__(self):
		return len(self.patients)

	def __getitem__(self, position):
		return Patient(self.patients[position])

	def __iter__(self):
		return PatientsIterator(self)

class PatientsIterator:

	def __init__(self, patients):
		self._index = 0
		self.patients = patients

	def __next__(self):
		if(self._index == len(self.patients)):
			raise StopIteration

		val = self.patients[self._index]
		self._index += 1
		return val