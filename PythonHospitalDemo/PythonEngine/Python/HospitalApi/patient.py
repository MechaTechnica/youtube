import builtins


class Patient:
	def __init__(self, patient):
		self.patient = patient
		self.name = self.patient.PatientName
		self.id = self.patient.Id
		self.age = self.patient.Age

	def __str__(self):
		return self.patient.PatientName