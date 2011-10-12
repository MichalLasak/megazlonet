/*********************************************************************
*
*      Copyright (C) 2003 Andrew Khan
*
* This library is free software; you can redistribute it and/or
* modify it under the terms of the GNU Lesser General Public
* License as published by the Free Software Foundation; either
* version 2.1 of the License, or (at your option) any later version.
*
* This library is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
* Lesser General Public License for more details.
*
* You should have received a copy of the GNU Lesser General Public
* License along with this library; if not, write to the Free Software
* Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
***************************************************************************/

// Port to C# 
// Chris Laforet
// Wachovia, a Wells-Fargo Company
// Feb 2010


namespace CSharpJExcel.Jxl.Biff.Drawing
	{
	/**
	 * The SpGr escher atom
	 */
	class Spgr : EscherAtom
		{
		/**
		 * The binary data
		 */
		private byte[] data;

		/**
		 * Constructor
		 *
		 * @param erd the raw escher record data
		 */
		public Spgr(EscherRecordData erd)
			: base(erd)
			{
			}

		/**
		 * Constructor
		 */
		public Spgr()
			: base(EscherRecordType.SPGR)
			{
			setVersion(1);
			data = new byte[16];
			}

		/**
		 * Gets the binary data
		 *
		 * @return the binary data
		 */
		public override byte[] getData()
			{
			return setHeaderData(data);
			}
		}
	}

